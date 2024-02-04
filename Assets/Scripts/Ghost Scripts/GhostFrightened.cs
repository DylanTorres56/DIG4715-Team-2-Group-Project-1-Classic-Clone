using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    public SpriteRenderer body;
    public SpriteRenderer blue;
    public SpriteRenderer white;
    Animator animator;

    public bool eaten { get; private set; }

    public override void Enable(float duration) 
    {
        base.Enable(duration);

        this.body.enabled = false;
        this.blue.enabled = true;
        this.white.enabled = false;

        Invoke(nameof(Flash), duration / 2.0f);
    }

    public override void Disable()
    {
        base.Disable();

        this.body.enabled = true;

        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void Eaten() 
    {
        this.eaten = true;
        //gameObject.GetComponent<Animator>().Play("dying");
        Vector3 position = this.ghost.home.insideTransform.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;
        
        Invoke(nameof(GhostDeath), 1.5f);

        this.body.enabled = false;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    private void GhostDeath()
    {
        this.ghost.home.Enable(this.duration);
    }

    // Update is called once per frame
    private void Flash()
    {
        if (!this.eaten) 
        {
            this.blue.enabled = false;
            this.white.enabled = true;
            //this.white.GetComponent<AnimatedSprite>().Restart();
        }

    }

    private void OnEnable()
    {
        this.ghost.movement.speedMultiplier = 0.5f;
        this.eaten = false;
    }

    private void OnDisable() 
    {
        this.ghost.movement.speedMultiplier = 1.0f;
        this.eaten = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pac-Man"))
        {
            if (this.enabled)
            {
                Eaten();
            }            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;

            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (this.ghost.target.position - newPosition).sqrMagnitude;

                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }
            //Sets new direction as whichever vector to Pac-Man is the shortest
            this.ghost.movement.SetDirection(direction);
        }

    }
}
