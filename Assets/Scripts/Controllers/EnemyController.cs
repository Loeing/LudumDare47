using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ICollection;

public class EnemyController : MoveController {


    //Need A*
    private Queue<Node> AStar()
    {

    }
    private Node BFS(Node target, Node root) 
    {
        Queue<Node> visited = new Queue<Node>();
        Queue<Node> discovered = new Queue<Node>();
        discovered.Enqueue(root);
        while(discovered.Count > 0)
        {
            Node visiting = discovered.Dequeue;
            if(visiting.Equals(target))//probably do something with occupier
            {
                return visiting;
            }

            foreach(Node neighbor in visiting.GetNeighbors())
            {
                if(!discovered.Contains(neighbor))
                {
                    discovered.Enqueue(neighbor);
                }
            }

        }

    }
}