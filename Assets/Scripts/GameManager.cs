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

    public int score { get; private set; }
    public int lives { get; private set; }

    public TextMeshProUGUI scoreText;

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
        foreach (Transform pellet in this.pellets) 
        {
            pellet.gameObject.SetActive(true);
        }

        ResetState();
        
    }

    private void ResetState() 
    {
        //Each ghost is set as true when a new round is called
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(true);
        }

        //Pac-Man is set as true when a new round is called
        this.pacMan.gameObject.SetActive(true);
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
    }

    private void SetScore(int currentScore)
    {
        score = currentScore;
    }

    private void SetLives(int currentLives) 
    {
        lives = currentLives;
    }

    public void GhostEaten(Ghost eatenGhost) 
    {
        SetScore(score + eatenGhost.points);
    }

    public void PacManEaten() 
    {
        this.pacMan.gameObject.SetActive(false);
        SetLives(lives - 1);

        if (lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f);
        }
        else 
        {
            GameOver();
        }
    }

    //NOTE: This is a potential holdover from the previous version of the GameManager, so I am commenting it out for the time being.

    //private void AddToScore(int amount) 
    //{
    //score += amount;
    //scoreText.text = "High Score: \n     " + score;
    //}

}
