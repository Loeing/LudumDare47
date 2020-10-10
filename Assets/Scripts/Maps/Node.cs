using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node //should it be a monobehavior if it's just holding our data?
{ 
    public Vector3 center; 
    public Vector2 gridPos;
    public Node Up {get; set;}
    public Node Down {get; set;}
    public Node Left {get; set;}
    public Node Right {get; set;}

    public Item item;
    public bool occupied = false;
    
    public bool passable = true;
    public MoveController occupier;
    private List<Node> neighbors;
    
    public List<Node> GetNeighbors() 
    {
        if(neighbors.Exists())
        {
            return neighbors;
        } else {
            neighbors = new List<Node>(Up, Down, Left, Right);
            neighbors.RemoveAll(node => node == null);
        }
    }
    public Dictionary<int, bool> passableTiles = new Dictionary<int, bool>(){
        { 1, true},
        { 2, false}
    };

    public Node(Vector2 gridPos, Vector3 center, int tileType)
    {
        this.center = center;
        this.gridPos = gridPos;
        this.passable = passableTiles[tileType];
    }

    public void Occupy()
    {

    }
    public bool IsPassable()
    {
        return passable && !occupied;
    }

}