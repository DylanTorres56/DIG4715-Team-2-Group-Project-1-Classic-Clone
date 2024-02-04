using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    //TO DO: GameManager is about ready-- next is reconfiguring Pac-Man GameObject (TIMESTAMP: https://youtu.be/TKt_VlMn_aA?t=2588)

    public Ghost[] ghosts;
        
    public PacMan pacMan;

    public Transform pellets;
    public Transform bonusFruit1;
    public Transform bonusFruit2;

    public int ghostMultiplier { get; private set;} = 1;
    public int score { get; private set; }
    public int lives { get; private set; }
    public int numOfPelletsEaten { get; private set; }
    public int numOfBonusFruitEaten { get; private set; } = 0;

    public TextMeshProUGUI scoreText;
    public Image gameOver;
    public Image gameWon;
    public Image life1;
    public Image life2;
    public Button menuButton;
    public Animator pacAC;

    //matthew- adding most sfx in gamemanager cause it seems like most of the functions are all in here!!!!!

    [SerializeField] private AudioClip[] pelletEatClips;
    [SerializeField] private AudioClip powerPelletEatClip;
    [SerializeField] private AudioClip ghostEatClip;
    [SerializeField] private AudioClip bonusFruitEatClip;
    [SerializeField] private AudioClip pacMaidDeathClip;
    [SerializeField] private AudioClip pacMaidGameOverClip;
    [SerializeField] private AudioClip roundStartClip;
    [SerializeField] private AudioClip roundWinClip;
    [SerializeField] private AudioClip menuMusicClip;
    [SerializeField] private AudioClip levelMusicClip;

    // Start is called before the first frame update
    void Start()
    {
        NewGame();
        bonusFruit1.gameObject.SetActive(false);
        bonusFruit2.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update() 
    {
        
    }

    private void NewGame() 
    {
        SetScore(0);
        SetLives(3);
        NewRound();
        SoundFXManager.Instance.PlayMusic(levelMusicClip, transform, 0.75f);
    }

    private void NewRound() 
    {
        //matthew - include only if we are able to pause time at the beginning of rounds like in the og!!!!
        //SoundFXManager.Instance.PlaySoundFXClip(roundStartClip, transform, 1f);

        //Pellets are turned back on when a new round is called
        gameOver.enabled = false;
        foreach (Transform pellet in this.pellets) 
        {
            pellet.gameObject.SetActive(true);
        }
        ResetState();
    }

    private void ResetState() 
    {
        this.pacMan.GetComponent<Collider2D>().enabled = true;
        ResetGhostMultiplier();
        //Each ghost is set as true when a new round is called
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }
        //Pac-Man is set as true when a new round is called
        pacMan.ResetState();
    }


    private void GameOver() 
    {
        //Each ghost is set as false when the game is over
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        StopAllAudio();
        SoundFXManager.Instance.PlaySoundFXClip(pacMaidGameOverClip, transform, 1f);
        //Pac-Man is set as false when the game is over
        this.pacMan.gameObject.SetActive(false);
        EndGame(1);
    }

    private void SetScore(int currentScore)
    {
        score = currentScore;
        scoreText.text = score.ToString();
    }

    private void SetLives(int currentLives) 
    {
        lives = currentLives;
    }

    public void GhostEaten(Ghost eatenGhost) 
    {
        SoundFXManager.Instance.PlaySoundFXClip(ghostEatClip, transform, 1f);
        SetScore(score + eatenGhost.points * this.ghostMultiplier);
        this.ghostMultiplier++;
    }

    public void PacManEaten() 
    {
        SetLives(lives - 1);
        this.ghosts[0].gameObject.SetActive(false);
        this.ghosts[1].gameObject.SetActive(false);
        this.ghosts[2].gameObject.SetActive(false);
        this.ghosts[3].gameObject.SetActive(false);
        pacAC.SetBool("PacDied", true);
        this.pacMan.GetComponent<Collider2D>().enabled = false;

        if (lives > 0)
        {
            StopAllAudio();
            SoundFXManager.Instance.PlaySoundFXClip(pacMaidDeathClip, transform, 1f);
            SoundFXManager.Instance.PlayMusic(levelMusicClip, transform, 1f);
            if (lives == 2)
            {
                life2.enabled = false;
            }
            else{
                life1.enabled = false;
            }
            Invoke(nameof(ResetState), 1.5f);
        }
        else 
        {
            GameOver();
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        numOfPelletsEaten++;

        //Game checks for pellets eaten-- after 70, the 1st bonus fruit appears and can be eaten, then for the 2nd at 170 and after the 1st is already gone
        if (numOfPelletsEaten >= 70 && numOfBonusFruitEaten == 0)
        {
            bonusFruit1.gameObject.SetActive(true);
        }
        if (numOfPelletsEaten >= 170 && numOfBonusFruitEaten == 1)
        {
            bonusFruit2.gameObject.SetActive(true);
        }

        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        //plays random pellet sounds
        SoundFXManager.Instance.PlayRandomSoundFXClip(pelletEatClips, transform, 1f);

        if (!HasRemainingPellets())
        {
            this.pacMan.gameObject.SetActive(false);
            for (int i = 0; i < ghosts.Length; i++) {
                ghosts[i].gameObject.SetActive(false);
            }
            EndGame(2);
            StopAllAudio();
            SoundFXManager.Instance.PlaySoundFXClip(roundWinClip, transform, 1f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        numOfPelletsEaten++;
        for (int i = 0; i < this.ghosts.Length; i++) 
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }
        PelletEaten(pellet);
        SoundFXManager.Instance.PlaySoundFXClip(powerPelletEatClip, transform, 1f);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    public void BonusFruitEaten(BonusFruit bonusFruit) 
    {
        SoundFXManager.Instance.PlaySoundFXClip(bonusFruitEatClip, transform, 1f);
        numOfBonusFruitEaten++;
        bonusFruit.gameObject.SetActive(false);
        SetScore(this.score + bonusFruit.fruitPoints);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
                return true;
        }
        return false;
    }

    private void EndGame(int state)
    {
        menuButton.gameObject.SetActive(true);
        Time.timeScale = 0;
        if (state <= 1)
        {
            gameOver.enabled = true;
        }
        else
        {
            gameWon.enabled = true;
        }
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }


    //Stop all sounds (for when game ends/wins and any sounds are playing)
    //credit @dpanov76mail-ru on discussions.unity.com
    private AudioSource[] allAudioSources;

    void StopAllAudio()
    {

        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
                       
            foreach (AudioSource audioS in allAudioSources)
            {
                audioS.Stop();
            }
       

    }

}
