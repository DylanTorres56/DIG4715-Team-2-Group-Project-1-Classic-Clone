using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeController : MonoBehaviour
{
    public bool canMoveLeft = false;
    public bool canMoveRight = false;
    public bool canMoveUp = false;
    public bool canMoveDown = false;

    public GameObject nodeLeft;
    public GameObject nodeRight;
    public GameObject nodeUp;
    public GameObject nodeDown;

    public bool isWarpLeftNode = false;
    public bool isWarpRightNode = false;
    
    //If node contains pellet on startup
    public bool isPelletNode = false;
    //If node still has a pellet
    public bool hasPellet = false;

    public SpriteRenderer pelletSprite;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (transform.childCount > 0) 
        {
            isPelletNode = true;
            hasPellet = true;
            pelletSprite = GetComponentInChildren<SpriteRenderer>();
        }

        //Detects nodes to move to on left side
        RaycastHit2D[] hitsLeft;
        hitsLeft = Physics2D.RaycastAll(transform.position, -Vector2.right);

        for (int i = 0; i < hitsLeft.Length; i++)
        {
            float distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
            if (distance < 0.8f && hitsLeft[i].collider.tag == "Node")
            {
                canMoveLeft = true;
                nodeLeft = hitsLeft[i].collider.gameObject;
            }
        }

        //Detects nodes to move to on right side
        RaycastHit2D[] hitsRight;
        hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right);

        for (int i = 0; i < hitsRight.Length; i++)
        {
            float distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
            if (distance < 0.8f && hitsRight[i].collider.tag == "Node")
            {
                canMoveRight = true;
                nodeRight = hitsRight[i].collider.gameObject;
            }
        }

        //Detects nodes to move to above
        RaycastHit2D[] hitsUp;
        hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up);

        for (int i = 0; i < hitsUp.Length; i++)
        {
            float distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
            if (distance < 0.8f && hitsUp[i].collider.tag == "Node")
            {
                canMoveUp = true;
                nodeUp = hitsUp[i].collider.gameObject;
            }
        }

        //Detects nodes to move to below
        RaycastHit2D[] hitsDown;
        hitsDown = Physics2D.RaycastAll(transform.position, -Vector2.up);

        for (int i = 0; i < hitsDown.Length; i++) 
        {
            float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
            if (distance < 0.8f && hitsDown[i].collider.tag == "Node") 
            {
                canMoveDown = true;
                nodeDown = hitsDown[i].collider.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetNodeFromDirection(string direction) 
    {
        if (direction == "left" && canMoveLeft)
        {
            return nodeLeft;
        }
        else if (direction == "right" && canMoveRight)
        {
            return nodeRight;
        }
        else if (direction == "up" && canMoveUp)
        {
            return nodeUp;
        }
        else if (direction == "down" && canMoveDown)
        {
            return nodeDown;
        }
        else
        {
            return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.tag == "Player" && hasPellet) 
        {
            hasPellet = false;
            pelletSprite.enabled = false;
            gameManager.CollectedPellet(this);
        }
    }

}