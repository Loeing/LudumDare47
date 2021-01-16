using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class EnemyController : MonoBehaviour 
{
    private MoveComponent moveComponent;

    protected void Start()
    {
        moveComponent = GetComponent<MoveComponent>();
        GameMaster.instance.enemies.Add(this);
    }

    protected void DestroyEntity()
    {
        GameMaster.instance.enemies.Remove(this);
    }

    public void MoveRand()
    {
        var rand = new System.Random();
        var dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
        Direction dir = dirs[rand.Next(dirs.Count)];
        Debug.Log("Moving enemy " + dir);
        moveComponent.Move(dir);
    }

    //Need A*
    //Currently limiting pathfinding within a room, then path from room to room
    private Queue<Node> AStar(start, end)
    {
        //TODO: rempace pseudocode with actual code
        //investigate this pq https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
        //possible optimization: pre-compute all the exact paths heuristics for each map (not good for changing maps)
        //create open list
        PriorityQueue open = new PriorityQueue<Node>();
        HashSet closed = new HashSet<Node>();
        open.Enqueue(start);

        while(!open.Empty)
        {
            Node current, var priority = open.Dequeue();
            //find all successors from the node
            var neighbors = current.neighbors;
            for(Node neighbor : neighbors)
            {
                //distance estimate between current node and target
                var h = ManhattanDistance(current, end);
                //plus distance between the considered node and the current node (always one turn away)
                var g = priority + 1;
                var f = h + g;

                //skip if node is in open list with a lower heuristic value
                if(open.Contains(neighbor))
                {
                    //not sure if this is an ok shortcut 
                    //should be, but maybe we should get the node from the queue anyway
                    if(f > neighbor.Heuristic)
                    {
                        continue;
                    }
                }
                //skip if node is in closed list with a lower heuristic value
                if(closed.Contains(neighbor))
                {
                    //same comment as above
                    if(f > neighbor.Heuristic)
                    {
                        continue;
                    }
                }
                neighbor.Heuristic = f;
                open.Enqueue(neighbor);
            }
            closed.Enqueue(current);
        }
    }
    //TODO: draw gizmos to see the planned path

    //oh noes need different heuristics for different movement types (i.e. solve the plunge puzzle)
    //need a good way to compute the neighbors as the ends for axis movement. that should do the trick
    private float ManhattanDistance(start, end)
    {
        return Math.Abs(start.gridPos.x - end.gridPos.x) + Math.Abs(start.gridPos.y - end.gridPos.y);
    }
    
    //dedicated pathfinding instead?
    private Node BFS(Node target, Node root) 
    {
        Queue<Node> visited = new Queue<Node>();
        Queue<Node> discovered = new Queue<Node>();
        discovered.Enqueue(root);
        while(discovered.Count > 0)
        {
            Node visiting = discovered.Dequeue();
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
        return null;
    }

    
}