using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    public enum GhostNodeStatesEnum 
    {
        respawning,
        leftNode,
        rightNode,
        centerNode,
        startNode,
        movingInNodes
    }

    public GhostNodeStatesEnum ghostNodeState;
    public GhostNodeStatesEnum respawnState;

    public enum GhostType
    {
        blinky,
        pinky,
        inky,
        clyde
    }

    public GhostType ghostType;

    public GameObject ghostNodeStart;
    public GameObject ghostNodeCenter;
    public GameObject ghostNodeLeft;
    public GameObject ghostNodeRight;

    public MovementController movementController;

    public GameObject startingNode;

    public bool readyToLeaveHome = false;

    public GameManager gameManager;


    public bool testRespawn = false;

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.blinky) 
        {
            ghostNodeState = GhostNodeStatesEnum.startNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeStart;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.pinky)
        {
            ghostNodeState = GhostNodeStatesEnum.centerNode;
            respawnState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.inky) 
        {
            ghostNodeState = GhostNodeStatesEnum.leftNode;
            respawnState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
            readyToLeaveHome = true;
        }
        else if (ghostType == GhostType.clyde) 
        {
            ghostNodeState = GhostNodeStatesEnum.rightNode;
            respawnState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
            readyToLeaveHome = true;
        }
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (testRespawn == true) 
        {
            readyToLeaveHome = false;
            ghostNodeState = GhostNodeStatesEnum.respawning;
            testRespawn = false;
        }
    }

    public void ReachedCenterOfNode(NodeController nodeController)
    {
        if (ghostNodeState == GhostNodeStatesEnum.movingInNodes)
        {
            //Determine next game node to go to
            if (ghostType == GhostType.blinky) 
            {
                DetermineBlinkyDirection();
            }
        }
        else if (ghostNodeState == GhostNodeStatesEnum.respawning)
        {
            Debug.Log("STATE IS NOW: RESPAWNING");
            Debug.Log("Blinky's X Position: " + transform.position.x + " | Blinky's Y Position: " + transform.position.y);
            Debug.Log("Start Node's X Position: " + ghostNodeStart.transform.position.x + " | Start Node's Y Position: " + ghostNodeStart.transform.position.y);

            string direction = "";

            //Ghost has reached start node- move to center node (TO DO: THIS DOESN'T WORK FOR SOME REASON, ASK CALL)
            if (transform.position.x == ghostNodeStart.transform.position.x && transform.position.y == ghostNodeStart.transform.position.y)
            {
                direction = "down";
                Debug.Log("MOVE DOWN");
            }
            //Ghost has reached center node- either finish respawn or move to left/right side
            else if (transform.position.x == ghostNodeCenter.transform.position.x && transform.position.y == ghostNodeCenter.transform.position.y)
            {
                if (respawnState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = respawnState;
                }
                else if (respawnState == GhostNodeStatesEnum.leftNode)
                {
                    direction = "left";
                }
                else if (respawnState == GhostNodeStatesEnum.rightNode)
                {
                    direction = "right";
                }
            }

            //If a ghost's respawn state is either the left or right node AND node has been found, leave home again
            else if (
                (transform.position.x == ghostNodeLeft.transform.position.x && transform.position.y == ghostNodeLeft.transform.position.y)
                || (transform.position.x == ghostNodeRight.transform.position.x && transform.position.y == ghostNodeRight.transform.position.y)
                )
            {
                ghostNodeState = respawnState;
            }
            //Ghosts are still in game board; locate start board
            else 
            {
                //Determine a ghost's quickest direction home
                direction = GetClosestDirection(ghostNodeStart.transform.position);
            }
                        
            movementController.SetDirection(direction);
        }
        else 
        {
            //If ghosts are ready to leave home
            if (readyToLeaveHome) 
            {
                //If ghosts are in left node, move to center & set direction
                if (ghostNodeState == GhostNodeStatesEnum.leftNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("right");
                }
                //If ghosts are in right node, move to center
                else if (ghostNodeState == GhostNodeStatesEnum.rightNode) 
                {
                    ghostNodeState = GhostNodeStatesEnum.centerNode;
                    movementController.SetDirection("left");
                }
                //If ghosts are in center node, move to start node
                else if (ghostNodeState == GhostNodeStatesEnum.centerNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.startNode;
                    movementController.SetDirection("up");
                }
                //If ghosts are in start node, start moving around in game
                else if (ghostNodeState == GhostNodeStatesEnum.startNode)
                {
                    ghostNodeState = GhostNodeStatesEnum.movingInNodes;
                    movementController.SetDirection("left");
                }
            }
        }
    }

    void DetermineBlinkyDirection() 
    {
        string direction = GetClosestDirection(gameManager.pacMan.transform.position);
        movementController.SetDirection(direction);
    }
    void DeterminePinkyDirection()
    {
    
    }
    void DetermineInkyDirection()
    {
    
    }
    void DetermineClydeDirection()
    {
    
    }

    string GetClosestDirection(Vector2 target) 
    {
        float shortestDistance = 0;
        string lastMovingDirection = movementController.lastMovingDirection;
        string newDirection = "";
        NodeController nodeController = movementController.currentNode.GetComponent<NodeController>();

        //If a ghost is moving up and can't reverse direction
        if (nodeController.canMoveUp && lastMovingDirection != "down") 
        {
            //Get node above a ghost
            GameObject nodeUp = nodeController.nodeUp;
            //Get distance between top node and Pac-Man
            float distance = Vector2.Distance(nodeUp.transform.position, target);

            //If this is the shortest distance so far, set as ghost's direction
            if (distance < shortestDistance || shortestDistance == 0) 
            {
                shortestDistance = distance;
                newDirection = "up";
            }
        }

        //If a ghost is moving down and can't reverse direction
        if (nodeController.canMoveDown && lastMovingDirection != "up")
        {
            //Get node above a ghost
            GameObject nodeDown = nodeController.nodeDown;
            //Get distance between top node and Pac-Man
            float distance = Vector2.Distance(nodeDown.transform.position, target);

            //If this is the shortest distance so far, set as ghost's direction
            if (distance < shortestDistance || shortestDistance == 0)
            {
                shortestDistance = distance;
                newDirection = "down";
            }
        }

        //If a ghost is moving left and can't reverse direction
        if (nodeController.canMoveLeft && lastMovingDirection != "right") 
        {
            //Get node above a ghost
            GameObject nodeLeft = nodeController.nodeLeft;
            //Get distance between top node and Pac-Man
            float distance = Vector2.Distance(nodeLeft.transform.position, target);

            //If this is the shortest distance so far, set as ghost's direction
            if (distance < shortestDistance || shortestDistance == 0) 
            {
                shortestDistance = distance;
                newDirection = "left";
            }
        }

        //If a ghost is moving right and can't reverse direction
        if (nodeController.canMoveRight && lastMovingDirection != "left") 
        {
            //Get node above a ghost
            GameObject nodeRight = nodeController.nodeRight;
            //Get distance between top node and Pac-Man
            float distance = Vector2.Distance(nodeRight.transform.position, target);

            //If this is the shortest distance so far, set as ghost's direction
            if (distance < shortestDistance || shortestDistance == 0) 
            {
                shortestDistance = distance;
                newDirection = "right";
            }
        }

        return newDirection;

    }
}
