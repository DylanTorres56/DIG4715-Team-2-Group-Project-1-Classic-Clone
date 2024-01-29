using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    public GameManager gameManager;

    public GameObject currentNode;
    public float speed = 8f;

    public string direction = "";
    public string lastMovingDirection = "";

    public bool canWarp = true;
    public bool isGhost = false;

    // Start is called before the first frame update
    void Awake()
    {        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        NodeController currentNodeController = currentNode.GetComponent<NodeController>();

        transform.position = Vector2.MoveTowards(transform.position, currentNode.transform.position, speed * Time.deltaTime);

        bool reverseDirection = false;
        if (
            (direction == "left" && lastMovingDirection == "right")
            || (direction == "right" && lastMovingDirection == "left")
            || (direction == "up" && lastMovingDirection == "down")
            || (direction == "down" && lastMovingDirection == "up")
            ) 
        {
            reverseDirection = true;
        }

        //Figures out if our position is the same as the current node's center
        if (transform.position.x == currentNode.transform.position.x && transform.position.y == currentNode.transform.position.y || reverseDirection)
        {
            if (isGhost) 
            {
                GetComponent<EnemyController>().ReachedCenterOfNode(currentNodeController);                
            }

            //Hitting leftWarpNode = Warp to right
            if (currentNodeController.isWarpLeftNode && canWarp)
            {
                currentNode = gameManager.rightWarpNode;
                direction = "left";
                lastMovingDirection = "left";
                transform.position = currentNode.transform.position;
                canWarp = false;
                Debug.Log("CURRENT NODE: " + currentNode);
            }
            //Hitting rightWarpNode = Warp to left
            else if (currentNodeController.isWarpRightNode && canWarp)
            {
                currentNode = gameManager.leftWarpNode;
                direction = "right";
                lastMovingDirection = "right";
                transform.position = currentNode.transform.position;
                canWarp = false;
                Debug.Log("CURRENT NODE: " + currentNode);
            }
            //Otherwise, find next node
            else
            {
                //If we are not a respawning ghost and we have reached the start node, stop us if we try to move down
                if (currentNodeController.isGhostStartingNode && direction == "down"
                    && (!isGhost || GetComponent<EnemyController>().ghostNodeState != EnemyController.GhostNodeStatesEnum.respawning)) 
                {
                    direction = lastMovingDirection;
                }


                //Next node determined from NodeController using current direction
                GameObject newNode = currentNodeController.GetNodeFromDirection(direction);

                //If we can move in the desired direction…
                if (newNode != null)
                {
                    currentNode = newNode;
                    lastMovingDirection = direction;
                    Debug.Log("CURRENT NODE: " + currentNode);
                }
                //If we can't move in the desired direction, check to keep going in the lastMovingDirection 
                else
                {
                    direction = lastMovingDirection;
                    newNode = currentNodeController.GetNodeFromDirection(direction);
                    if (newNode != null)
                    {
                        currentNode = newNode;
                        Debug.Log("CURRENT NODE: " + currentNode);
                    }
                }
            }

        }
        //NOT at node's center
        else 
        {
            canWarp = true;
        }        
    }

    public void SetDirection(string newDirection) 
    {
        direction = newDirection;
    }    

}
