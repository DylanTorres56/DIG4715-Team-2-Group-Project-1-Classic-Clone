using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pacMan;

    public GameObject leftWarpNode;
    public GameObject rightWarpNode;

    public AudioSource siren;
    public AudioSource munch1;
    public AudioSource munch2;
    public int currentMunch;

    public int score;
    public TextMeshProUGUI scoreText;

    // Start is called before the first frame update
    void Awake()
    {
        score = 0;
        currentMunch = 0;
        siren.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddToScore(int amount) 
    {
        score += amount;
        scoreText.text = "Score: " + score;
    }

    public void CollectedPellet(NodeController nodeController) 
    {
        if (currentMunch == 0) 
        {
            munch1.Play();
            currentMunch = 1;
        }
        else if (currentMunch == 1) 
        {
            munch2.Play();
            currentMunch = 0;
        }

        AddToScore(10);

        //Add to score

        //Check for remaining pellets

        //Check how many pellets were eaten

        //Check if a pellet is a Power Pellet
    }
}
