using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
    public float speed = 8.0f;
    public float speedMultiplier = 1.0f;
    public Vector2 initialDirection;
    public LayerMask obstacle;

    public Rigidbody2D rigidbody{get; private set;}

    public Vector2 direction {get; private set;}
    public Vector2 nextDirection {get; private set;}
    public Vector3 startingPosition {get; private set;}

    public string entityName;
    Animator animator;

    private void Awake()
    {
        this.rigidbody = GetComponent<Rigidbody2D>();
        this.startingPosition = this.transform.position;
    }

    private void Start()
    {
        ResetState();
    }

    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction =  this.initialDirection;
        this.transform.position = this.startingPosition;
        this.rigidbody.isKinematic = false;
        this.enabled = true;
    }

    private void Update()
    {
        if (this.nextDirection != Vector2.zero) 
        {
            SetDirection(this.nextDirection);
            if(string.Compare(entityName, "Pac-Maid") != 0)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                if (direction.x > 0)
                {
                    gameObject.GetComponent<Animator>().Play("right");
                    Debug.Log("Moving Right");
                }
                else if (direction.x < 0)
                {
                    gameObject.GetComponent<Animator>().Play("left");
                   Debug.Log("Moving Left");
                }
            }   
        } 
    }

    private void FixedUpdate()
    {
        Vector2 position = this.rigidbody.position;
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;
        this.rigidbody.MovePosition(position +  translation);
    }

    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        }
        else
        {
            this.nextDirection = direction;
        }
    }

    public bool Occupied(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacle);
        return hit.collider != null;
    }
}
