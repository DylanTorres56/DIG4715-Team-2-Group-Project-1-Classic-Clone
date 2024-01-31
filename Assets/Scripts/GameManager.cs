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

    public int ghostMultiplier { get; private set;} = 1;
    public int score { get; private set; }
    public int lives { get; private set; }

    public TextMeshProUGUI scoreText;
    public Image gameOver;
    public Image gameWon;
    public Image life1;
    public Image life2;
    public Button menuButton;

    // Start is called before the first frame update
    void Start()
    {
        NewGame();
    }

    // Update is called once per frame
    private void Update() 
    {
        if (lives <= 0 && Input.anyKeyDown) 
        {
            NewGame();
        }
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
        endGame(1);
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
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);

        if (!HasRemainingPellets())
        {
            this.pacMan.gameObject.SetActive(false);
            for (int i = 0; i < ghosts.Length; i++) {
                ghosts[i].gameObject.SetActive(false);
            }
            endGame(2);
            //Disabling new round for now, just putting up a win screen - Chelle
            //Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        //TODO: Change ghost state.
        PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
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

    private void endGame(int state)
    {
        menuButton.gameObject.SetActive(true);
        Time. timeScale = 0;
        if (state <= 1)
        {
            gameOver.enabled = true;
        }
        else if(state > 1)
        {
            gameWon.enabled = true;
        }
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }
    //NOTE: This is a potential holdover from the previous version of the GameManager, so I am commenting it out for the time being.

    //private void AddToScore(int amount) 
    //{
    //score += amount;
    //scoreText.text = "High Score: \n     " + score;
    //}

}
