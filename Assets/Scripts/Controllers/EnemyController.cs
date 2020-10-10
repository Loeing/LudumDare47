using UnityEngine;
using System;
using System.Linq;

public class EnemyController : MoveController {
    
    MoveController target;

    void Start()
    {
        GameMaster.instance.enemies.Add(this);
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        base.Start();
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
}