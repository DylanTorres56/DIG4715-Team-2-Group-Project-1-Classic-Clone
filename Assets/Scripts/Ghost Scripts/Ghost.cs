using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Movement movement {get; private set;}
    public GhostHome home {get; private set;}
    public GhostScatter scatter {get; private set;}
    public GhostChase chase {get; private set;}
    public GhostFrightened frightened {get; private set;}

    public GhostBehavior initialBehavior;
    public Transform target;
    //TO DO: GameManager is about ready-- next is reconfiguring Pac-Man GameObject (TIMESTAMP: https://youtu.be/TKt_VlMn_aA?t=2588)
    public int points = 200;
    //Get done, loser.
    
    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();

    }
    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();
        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();
        if (this.home != this.initialBehavior)
        {
            this.home.Disable();
        }

        if (this.initialBehavior != null) 
        {
            this.initialBehavior.Enable();
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pac-Man"))
        {
            if(this.frightened.enabled)
            {
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                FindObjectOfType<GameManager>().PacManEaten();
            }
        }
    }
}
