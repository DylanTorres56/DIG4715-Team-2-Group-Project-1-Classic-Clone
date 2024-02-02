using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    Animator animator;
    public Movement movement {get; private set;}
    private void Awake()
    {
        this.movement = GetComponent<Movement>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            gameObject.GetComponent<Animator>().Play("up");
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            gameObject.GetComponent<Animator>().Play("left");
            this.movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            gameObject.GetComponent<Animator>().Play("down");
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            gameObject.GetComponent<Animator>().Play("right");
            this.movement.SetDirection(Vector2.right);
        }
    }

    public void ResetState()
    {
        this.gameObject.SetActive(true);
        this.movement.ResetState();
    }
}
