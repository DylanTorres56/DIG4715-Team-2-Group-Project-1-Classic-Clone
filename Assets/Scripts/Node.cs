using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public LayerMask obstacle;
    public List<Vector2> availableDirections { get; private set; }

    // Start is called before the first frame update
    private void Start()
    {
        this.availableDirections = new List<Vector2>();

        CheckAvailableDirection(Vector2.up);
        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    private void CheckAvailableDirection(Vector2 direction) 
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.0f, this.obstacle);
        if (hit.collider == null) 
        {
            this.availableDirections.Add(direction);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
