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
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            animator.SetFloat("PacMoveX", 0);
            animator.SetFloat("PacMoveY", 1);
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            animator.SetFloat("PacMoveX", -1);
            animator.SetFloat("PacMoveY", 0);
            this.movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            animator.SetFloat("PacMoveX", 0);
            animator.SetFloat("PacMoveY", -1);
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            animator.SetFloat("PacMoveX", 1);
            animator.SetFloat("PacMoveY", 0);
            this.movement.SetDirection(Vector2.right);
        }
    }

    public void ResetState()
    {
        animator.SetBool("PacDied", false);
        this.gameObject.SetActive(true);
        this.movement.ResetState();
    }
}
