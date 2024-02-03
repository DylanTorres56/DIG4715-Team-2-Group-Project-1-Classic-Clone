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
    public TextMeshProUGUI endScore;
    public TextMeshProUGUI endScore2;

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
    }

    private void NewRound() 
    {
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
        SetScore(score + eatenGhost.points * this.ghostMultiplier);
        this.ghostMultiplier++;
    }

    public void PacManEaten() 
    {
        this.pacMan.gameObject.SetActive(false);
        SetLives(lives - 1);
        
        if (lives > 0)
        {
            if (lives == 2)
            {
            life2.enabled = false;
            }
            if (lives == 1)
            {
                life1.enabled = false;
            }
            Invoke(nameof(ResetState), 3.0f);
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

        if (!HasRemainingPellets())
        {
            this.pacMan.gameObject.SetActive(false);
            for (int i = 0; i < ghosts.Length; i++) {
                ghosts[i].gameObject.SetActive(false);
            }
            EndGame(2);
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
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    public void BonusFruitEaten(BonusFruit bonusFruit) 
    {
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
        Time. timeScale = 0;
        if (state <= 1)
        {
            gameOver.enabled = true;
        }
        else
        {
            endScore.enabled = true;
            endScore2.enabled = true;
            gameWon.enabled = true;
        }
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
    
}
