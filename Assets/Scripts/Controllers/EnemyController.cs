using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public class EnemyController : MoveController {
    
    MoveController target;

    protected override void Start()
    {
        GameMaster.instance.enemies.Add(this);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        base.Start();
    }

    protected override void DestroyEntity()
    {
        GameMaster.instance.enemies.Remove(this);
        base.DestroyEntity();
    }
    public void MoveCloser()
    {
        Vector2 targetPos = target.currentNode.gridPos;
    }

    public void MoveRand()
    {
        var rand = new System.Random();
        var dirs = Enum.GetValues(typeof(Direction)).Cast<Direction>().ToList();
        Direction dir = dirs[rand.Next(dirs.Count)];
        Debug.Log("Moving enemy " + dir);
        Move(dir);
    }

    //Need A*
    // private Queue<Node> AStar()
    // {

    // }
    
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