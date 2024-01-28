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

    // Start is called before the first frame update
    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        movementController = GetComponent<MovementController>();
        if (ghostType == GhostType.blinky) 
        {
            ghostNodeState = GhostNodeStatesEnum.startNode;
            startingNode = ghostNodeStart;
        }
        else if (ghostType == GhostType.pinky)
        {
            ghostNodeState = GhostNodeStatesEnum.centerNode;
            startingNode = ghostNodeCenter;
        }
        else if (ghostType == GhostType.inky) 
        {
            ghostNodeState = GhostNodeStatesEnum.leftNode;
            startingNode = ghostNodeLeft;
        }
        else if (ghostType == GhostType.clyde) 
        {
            ghostNodeState = GhostNodeStatesEnum.rightNode;
            startingNode = ghostNodeRight;
        }
        movementController.currentNode = startingNode;
        transform.position = startingNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
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
            //Determine quickest direction to home
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
        string direction = GetClosestToDirection(gameManager.pacMan.transform.position);
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

    string GetClosestToDirection(Vector2 target) 
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
