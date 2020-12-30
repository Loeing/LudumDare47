using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class Room : MonoBehaviour 
{

    public int Key { get; set; } 
    public Node[,] NodeGrid { get; set; }
    public int[,] TileGrid { get; set; }

    public List<GameObject> Entities { get; } = new List<GameObject>();
    //public List<GameObject> Passage { get; set; }
    public List<GameObject> Items { get; set; }

    public Room RoomInit(int key, int[,] tileGrid, Node[,] nodeGrid, LevelsData.EntityData[] entityData, LevelsData.PassageData[] passageData)
    {
        this.Key = key;
        this.TileGrid = tileGrid;
        this.NodeGrid = nodeGrid;
        InitGridGraph();
        // Debug.Log(entityData);
        InitEntities(entityData);
        InitPassages(passageData);
        return this;
    }

    private void InitGridGraph()
    {
        for(int x=0; x<=NodeGrid.GetUpperBound(0); x++)
        {
            for(int y=0; y<=NodeGrid.GetUpperBound(1); y++)
            {
                var upIndex = y + 1;
                var downIndex = y - 1;
                var rightIndex = x + 1;
                var leftIndex = x - 1;
                if(upIndex >= 0 && upIndex <= NodeGrid.GetUpperBound(1))
                    NodeGrid[x,y].Up = NodeGrid[x, upIndex];
                if(downIndex >= 0 && downIndex <= NodeGrid.GetUpperBound(1))
                    NodeGrid[x,y].Down = NodeGrid[x, downIndex];
                if(rightIndex >= 0 && rightIndex <= NodeGrid.GetUpperBound(0))
                    NodeGrid[x,y].Right = NodeGrid[rightIndex, y];
                if(leftIndex >= 0 && leftIndex <= NodeGrid.GetUpperBound(0))
                    NodeGrid[x,y].Left = NodeGrid[leftIndex, y];
            }
        }
    }

    //Do we want entity gameobjects or entities? probably objs so we can instantiate them
    //will it work if we instantiate from the entity prefab?
    private void InitEntities(LevelsData.EntityData[] entityData)
    {
        // Debug.Log("this is the EntityData");
        // Debug.Log(entityData);
        foreach(LevelsData.EntityData entity in entityData)
        {
            var entityPath = "Prefabs/" + entity.name;
            GameObject entityObj = (GameObject) Resources.Load(entityPath);
            // Entity ent = entityObj.GetComponent<Entity>();
            // Debug.Log("init ent");
            // Debug.Log(ent);
            //Node placementNode = NodeGrid[entity.x, entity.y];
            PlaceInstance(entityObj, entity.x, entity.y);
            // placement.Occupy(ent);
            // ent.currentNode = placement;
            // Debug.Log(ent.currentNode);
            // Debug.Log()
            // Entities.Add(entityObj);
        }
    }

    public void PlaceInstance(GameObject go, int x,  int y, int z=-1)
    {
        Node placementNode = NodeGrid[x, y];
        Vector3 placement = new Vector3(placementNode.center.x , placementNode.center.y, z);
        GameObject instance = Instantiate(go, placement, Quaternion.identity);
        Entity ent = instance.GetComponent<Entity>();
        ent.currentNode = placementNode;
        placementNode.Occupy(ent);
        Entities.Add(instance);
    }

    private void InitPassages(LevelsData.PassageData[] passageData)
    {
        foreach (var passage in passageData)
        {
            Node src = NodeGrid[passage.srcX, passage.srcY];
            src.Passage = new Passage(passage.destRoom, passage.destX, passage.destY);
        }
    }
}