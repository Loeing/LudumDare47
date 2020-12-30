using UnityEngine;

public class Mover: Entity
{
    // public void Move(Direction dir, bool axisMove=false)
    // {
    //     Node curNode = currentNode;
    //     curNode.Leave();
    //     Node endNode = curNode;
    //     if(axisMove)
    //     {
    //         endNode =  AxisMove(dir, curNode);
    //     } else {
    //         endNode = DirectMove(dir, curNode);
    //     }
    //     //StartCoroutine(SmoothMovement(endNode.center)); //add a bounce when not going anywhere
    //     currentNode = endNode;
    //     currentNode.Occupy(this);
    //     SharpMovement(endNode.center);
    // }

    // public bool IsOpponent(Entity other)
    // {
    //     //probably needs a per type override
    //     //TODO: also check for specific tag to handle allies
    //     return Entity.tag != other.tag;
    // }

    // private Node DirectMove(Direction dir, Node node)
    // {
    //     Node endNode = CheckMove(dir, node);
    //     if(!endNode.IsPassable())
    //     {   if(endNode.occupier)
    //         {
    //             if(IsOpponent(endNode.occupier))
    //             {
    //                 endNode.occupier.LoseHealth(1);     
    //             }
    //         }

    //         //TODO: add non passable options like combat
    //         endNode = node;
    //     }
    //     return endNode;
    // }

    // private Node AxisMove(Direction dir, Node node)
    // {
    //     var curNode = node;
    //     var nextNode = CheckMove(dir, node);
    //     while(nextNode != curNode)
    //     {
    //         if(nextNode.IsPassable())
    //         {
    //             curNode = nextNode;
    //             nextNode = CheckMove(dir, curNode);
    //         }
    //     }
    //     return curNode;
    // }

    // private Node CheckMove(Direction dir, Node node)
    // {
    //     if(dir == Direction.Up)
    //     {
    //         if(node.Up != null) 
    //         {
    //             return node.Up;
    //         }
    //     }
        
    //     if(dir == Direction.Down)
    //     {
    //         if(node.Down != null) 
    //         {
    //             return node.Down;
    //         }
    //     }
    //     if(dir == Direction.Left)
    //     {
    //         if(node.Left != null) 
    //         {
    //             return node.Left;
    //         }
    //     } 

    //     if(dir == Direction.Right)
    //     {
    //         if(node.Right != null) 
    //         {
    //             return node.Right;
    //         }
    //     }
    //     return node;
    // }

    // private void SharpMovement(Vector3 end)
    // {
    //     end.z = -2;
    //     transform.position = end;
    // }
}