using UnityEngine;
using System.Collections;


public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public class MoveComponent : MonoBehaviour 
{
    [SerializeField]
    private float tileDistance = 1f;
    [SerializeField]
    private float moveTime = 0.05f;

    [SerializeField]
    private int entityKey;
    [SerializeField]
    private Entity entity;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;
    private GameMaster gm;
    private bool inPassage = false; 

    private void Start() 
    {
        boxCollider = GetComponent <BoxCollider2D> ();
        rb2D = GetComponent <Rigidbody2D> ();
        entity = GetComponent<Entity>(); // i think this might be sufficient?
        gm = GameMaster.instance;
    }

    private void Update() 
    {
        
        
    }

    //Might not even need this. Am I overthinking?
    private void SetEntity()
    {
        // pull from the gm entity key map 
        // each prefab will be assigned a key
    }

    public void Move(Direction dir, bool axisMove=false)
    {
        Node curNode = entity.currentNode;
        curNode.Leave();
        Node endNode = curNode;
        if(axisMove)
        {
            endNode =  AxisMove(dir, curNode);
        } else {
            endNode = DirectMove(dir, curNode);
        }
        //StartCoroutine(SmoothMovement(endNode.center)); //add a bounce when not going anywhere
        entity.currentNode = endNode;
        entity.currentNode.Occupy(entity);
        if(entity.currentNode.Passage != null)
        {
            if(!inPassage) 
            {
                //Debug.Log("Leaving Room");
                this.LeaveRoom(entity.currentNode.Passage);
                inPassage = true;
            }
        } else {
            inPassage = false;
        }
    }

    public void LeaveRoom(Passage passage)
    {
        if(passage.destRoom != entity.currentNode.room)
        {
            if(gm == null)
            {
                gm = GameMaster.instance;
            }
            Room curRoom = gm.rooms[entity.currentNode.room];
            curRoom.Entities.Remove(this.gameObject);
            entity.currentNode.Leave(); //leave passage
            Room destRoom = gm.rooms[passage.destRoom];
            destRoom.Entities.Add(this.gameObject);
            entity.currentNode = destRoom.NodeGrid[passage.destX, passage.destY];
            entity.currentNode.Occupy(entity);

            // encapsulate the whole hiding and drawing stuff?
            //this is bad but whatever
            if(entity.Visible)
            {
                entity.Erase();
            }
            if(destRoom.Key == gm.board.currentRoom.Key)
            {
                entity.Draw();
            }

            //abstract out the movement function?
            SharpMovement(entity.currentNode.center);
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
        //Debug.Log("Team: " + this.team + " room: " + currentNode.room + " current node: " + currentNode.gridPos + " target node: " + endNode.gridPos + " dir: " + dir);
        if(!endNode.IsPassable())
        {   
            //Debug.Log("Impassible node: " + endNode.gridPos);
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

    private Node CheckMove(Direction dir, Node node)
    {
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
    }
}