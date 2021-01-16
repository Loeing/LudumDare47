using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node //should it be a monobehavior if it's just holding our data?
{ 
    public int room;
    public Vector3 center; 
    public Vector2 gridPos;
    public Node Up {get; set;}
    public Node Down {get; set;}
    public Node Left {get; set;}
    public Node Right {get; set;}

    public Item item;
    public bool occupied = false;
    
    public Passage Passage {get; set;} = null;

    public bool passable = true;
    public Entity occupier;
    private List<Node> neighbors;

    /*****FOR PATHFINDING*****/
    //maybe we shouldn't add this functionality here and instead have another class for pathfinding, but honestly, this seems fine
    //can't do anything in parrallel except with deep copy, but that's ok
    public float Heuristic = float.MaxValue;
    public Node Parent;

    
    public List<Node> GetNeighbors() 
    {
        if(neighbors!=null && neighbors.Any())
        {
            return neighbors;
        } 
        neighbors = new List<Node>(){Up, Down, Left, Right};
        neighbors.RemoveAll(node => node == null);
        return neighbors;
    }
    
    public Dictionary<int, bool> passableTiles = new Dictionary<int, bool>(){
        { 0, false},
        { 1, true},
        { 2, false},
        { 3, true}
    };

    public Node(int room, Vector2 gridPos, Vector3 center, int tileType)
    {
        this.room =  room;
        this.center = center;
        this.gridPos = gridPos;
        this.passable = passableTiles[tileType];
    }

    public void Occupy(Entity occupier)
    {
        this.occupier = occupier;
        this.occupied = true;
        occupier.currentNode = this;
    }

    public void Leave()
    {
        this.occupier = null;
        this.occupied = false;
    }

    public bool IsPassable()
    {
        return passable && !occupied;
    }

}