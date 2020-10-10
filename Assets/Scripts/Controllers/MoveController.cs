using UnityEngine;
using System.Collections;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
public abstract class MoveController : MonoBehaviour {
    
    [SerializeField]
    private float tileDistance = 1f;
    public float moveTime = 0.05f;

    [SerializeField]
    public Node currentNode; 

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;


    protected virtual void Start() {
        //Get a component reference to this object's BoxCollider2D
        boxCollider = GetComponent <BoxCollider2D> ();

        //Get a component reference to this object's Rigidbody2D
        rb2D = GetComponent <Rigidbody2D> ();

        //By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
        inverseMoveTime = 1f / moveTime;
    }

    public virtual void Move(Direction dir, bool axisMove=false)
    {
        Node endNode = currentNode;
        if(axisMove)
        {
            endNode =  AxisMove(dir, currentNode);
        } else {
            endNode = CheckMove(dir, currentNode);
            if(!endNode.IsPassable())
            {
                endNode = currentNode;
                //TODO: add non passable options like combat
            }
            SharpMovement(endNode.center);
        }
        //StartCoroutine(SmoothMovement(endNode.center)); //add a bounce when not going anywhere
        currentNode = endNode;
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
        Debug.Log("Current Node:");
        Debug.Log(node.gridPos);
        Debug.Log("Direction Node");
        if(dir == Direction.Up)
        {
            if(node.Up != null) 
            {
                Debug.Log(node.Up.gridPos);
                Debug.Log(node.Up.center);
                return node.Up;
            }
        }
        
        if(dir == Direction.Down)
        {
            if(node.Down != null) 
            {
                Debug.Log(node.Down.gridPos);
                Debug.Log(node.Down.center);
                return node.Down;
            }
        }
        if(dir == Direction.Left)
        {
            if(node.Left != null) 
            {
                Debug.Log(node.Left.gridPos);
                Debug.Log(node.Left.center);
                return node.Left;
            }
        } 

        if(dir == Direction.Right)
        {
            if(node.Right != null) 
            {
                Debug.Log(node.Right.gridPos);
                Debug.Log(node.Right.center);
                return node.Right;
            }
        }
        return node;
    }

    private void SharpMovement(Vector3 end)
    {
        end.z = -2;
        transform.position = end;
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