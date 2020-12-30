using UnityEngine;
using System.Collections;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}


//TODO: instead of an innheritance system, each part should be a component that the overall controller talks to. Would make for easier debugging

//TODO: controllers should have an entity, not inherit from them
// why though? so we can init entities with variables?
// ugh why am I doing this to support that dumb json loader?
public abstract class MoveController : Entity {
    
    [SerializeField]
    private float tileDistance = 1f;
    public float moveTime = 0.05f;



    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;
    private GameMaster gm;
    private bool inPassage = false; 
    



    protected virtual void Start() {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent <BoxCollider2D> ();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent <Rigidbody2D> ();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
        
        gm = GameMaster.instance;
    }

    public virtual void Move(Direction dir, bool axisMove=false)
    {
        //Debug.Log(this.currentNode);
        Node curNode = this.currentNode;
        //Debug.Log(curNode);
        curNode.Leave();
        Node endNode = curNode;
        if(axisMove)
        {
            endNode =  AxisMove(dir, curNode);
        } else {
            endNode = DirectMove(dir, curNode);
        }
        //StartCoroutine(SmoothMovement(endNode.center)); //add a bounce when not going anywhere
        currentNode = endNode;
        currentNode.Occupy(this);
        // Debug.Log("Team: " + this.team);
        // Debug.Log("Destination coord: " + currentNode.gridPos);
        // Debug.Log("Destination center: " + currentNode.center);
        // Debug.Log("Passage here: " + currentNode.Passage);
        if(currentNode.Passage != null)
        {
            if(!inPassage) 
            {
                Debug.Log("Leaving Room");
                this.LeaveRoom(currentNode.Passage);
                inPassage = true;
            }
        } else {
            inPassage = false;
        }
    }

    public void LeaveRoom(Passage passage)
    {
        if(passage.destRoom != currentNode.room)
        {
            if(gm == null)
            {
                gm = GameMaster.instance;
            }
            Debug.Log(gm);
            Debug.Log(gm.rooms);
            Room curRoom = gm.rooms[currentNode.room];
            curRoom.Entities.Remove(this.gameObject);
            currentNode.Leave(); //leave passage
            Room destRoom = gm.rooms[passage.destRoom];
            destRoom.Entities.Add(this.gameObject);
            this.currentNode = destRoom.NodeGrid[passage.destX, passage.destY];
            this.currentNode.Occupy(this);
            //this is bad but whatever
            if(this.Visible)
            {
                this.Erase();
            }
            if(destRoom.Key == gm.board.currentRoom.Key)
            {
                this.Draw();
            }
            SharpMovement(this.currentNode.center);
        }
    }

    public bool IsOpponent(GameObject other)
    {
        //probably needs a per type override
        //TODO: also check for specific tag to handle allies
        return gameObject.tag != other.tag;
    }

    private Node DirectMove(Direction dir, Node node)
    {
        Node endNode = CheckMove(dir, node);
        Debug.Log("Team: " + this.team + " room: " + currentNode.room + " current node: " + currentNode.gridPos + " target node: " + endNode.gridPos + " dir: " + dir);
        if(!endNode.IsPassable())
        {   
            Debug.Log("Impassible node: " + endNode.gridPos);
            if(endNode.occupier)
            {
                if(IsOpponent(endNode.occupier.gameObject))
                {
                    //endNode.occupier.LoseHealth(1);     
                }
            }
            
            //TODO: add non passable options like combat
            endNode = node;
        }
        SharpMovement(endNode.center);
        return endNode;
    }

    private Node AxisMove(Direction dir, Node node)
    {
        var curNode = node;
        var nextNode = CheckMove(dir, node);
        while(nextNode != curNode)
        {
            if(nextNode.IsPassable())
            {
                curNode = nextNode;
                nextNode = CheckMove(dir, curNode);
            }
        }
        return curNode;
    }

    //resolve moving through something
    private void AttemptMoveTo(Node node) {
        if(node.IsPassable())
        {
            //isPassa
        }
    }

    private Node CheckMove(Direction dir, Node node)
    {
        //Debug.Log("Current Node: " + node.gridPos);
        if(dir == Direction.Up)
        {
            if(node.Up != null) 
            {
                return node.Up;
            }
        }
        
        if(dir == Direction.Down)
        {
            if(node.Down != null) 
            {
                return node.Down;
            }
        }
        if(dir == Direction.Left)
        {
            if(node.Left != null) 
            {
                return node.Left;
            }
        } 

        if(dir == Direction.Right)
        {
            if(node.Right != null) 
            {
                return node.Right;
            }
        }
        return node;
    }

    private void SharpMovement(Vector3 end)
    {
        end.z = -2;
        transform.position = end;
        //Debug.Log("Move Pos: " + end);
    }

    //Shamelessly grabbed from the 2D roguelike tutorial
    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
        //Square magnitude is used instead of magnitude because it's computationally cheaper.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //While that distance is greater than a very small amount (Epsilon, almost zero):
        while(sqrRemainingDistance > float.Epsilon)
        {
            //Find a new position proportionally closer to the end, based on the moveTime
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

            //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
            rb2D.MovePosition (newPostion);

            //Recalculate the remaining distance after moving.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;

            //Return and loop until sqrRemainingDistance is close enough to zero to end the function
            yield return null;
        }
        Debug.Log("Done Moving Smoothly");
    }



}