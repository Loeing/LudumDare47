using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
//using Math;


public class Board : MonoBehaviour
{

    public static Board instance = null;

    //TODO: turn this into some kind of tile dict
    [SerializeField]
    private TileBase floor;
    [SerializeField]
    private TileBase background;
    [SerializeField]
    private TileBase passage;
    [SerializeField]
    private TileBase obstacle;

    [SerializeField]
    private GameObject playerCharacter;

    [SerializeField]
    private GameObject enemyCharacter;


    private Dictionary<int, TileBase> tiles = new Dictionary<int, TileBase>();
    private Tilemap baseTilemap;
    private Dictionary<int, LevelsData.LevelData> _levelsData;
    private LevelDataLoader _dataLoader;
    public Room currentRoom;
    private Node[,] grid;
    

    private void Awake() 
    {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        _dataLoader = GetComponent<LevelDataLoader>();    
        //This should be some kind of tile dict
        tiles.Add(0, background);
        tiles.Add(1, floor);
        tiles.Add(2, obstacle);
        tiles.Add(3, passage);
    }

    private void Start() 
    {
        baseTilemap = GetComponentsInChildren<Tilemap>()[0];
    }

    //TODO: load room objects, place entities and items

    // Start is called before the first frame update
    // public void InitGame(int level)
    // {
    //     //load levels
    //     _levelsData = _dataLoader.LoadLevelsData();  
    //     Debug.Log(_levelsData.Count + " levels have been stored in the dictionary!");
    //     baseTilemap = GetComponentsInChildren<Tilemap>()[0];
    //     var mapData = ListToMap(_levelsData[level].tiles, _levelsData[level].rowLength);
    //     MapFunctions.RenderMap(mapData, baseTilemap, tiles);
    //     initGridGraph();
    //     Vector3Int cellPosition = baseTilemap.WorldToCell(transform.position);
    //     Vector3 startPos = baseTilemap.GetCellCenterWorld(cellPosition);
    //     startPos.z = -1;
    //     //Vector3 startPos = new Vector3Int(0,0,0);
    //     GameObject pc = Instantiate(playerCharacter, startPos, Quaternion.identity);
    //     pc.GetComponent<PlayerController>().currentNode = grid[0,0];

    //     GameObject enemyOne = Instantiate(enemyCharacter, new Vector3(5.5f,5.5f,-1f), Quaternion.identity);
    //     enemyOne.GetComponent<EnemyController>().currentNode = grid[6,6];
    // }

    public void Draw(Room room)
    {
        currentRoom = room;
        grid = room.NodeGrid;
        //Debug.Log(room.TileGrid);
        MapFunctions.RenderMap(room.TileGrid, baseTilemap, tiles);
        foreach(GameObject entity in room.Entities)
        {
            Entity entData = entity.GetComponent<Entity>();
            entData.Draw();
            //PlaceInstance(entity, (int)entData.currentNode.gridPos.x, (int)entData.currentNode.gridPos.y);
        }        
    }

    public void Erase()
    {
        foreach(GameObject entity in currentRoom.Entities)
        {
            Entity entData = entity.GetComponent<Entity>();
            entData.Erase();
        } 
    }

    public void PlaceInstance(GameObject go, int x,  int y, int z=-1)
    {
        Node placementNode = grid[x, y];
        Vector3 placement = new Vector3(placementNode.center.x , placementNode.center.y, z);
        GameObject instance = Instantiate(go, placement, Quaternion.identity);
    }

    private int[,] ListToMap(int room, int[] tiles, int rowLength)
    {
        int rows = (int)Math.Ceiling((double) tiles.Length/(double)rowLength);
        int[,] result = new int[rows, rowLength];
        grid = new Node[rows, rowLength];
        int x = 0;
        int y = 0;
        for(int i=0; i < tiles.Length; i++)
        {
            if(x >= rowLength)
            {
                x = 0;
                y++;
            }
            result[x,y] = tiles[i];
            Vector3Int cellPosition = baseTilemap.WorldToCell(new Vector3(x,y,0));
            Vector3 centerPos = baseTilemap.GetCellCenterWorld(cellPosition);
            //Debug.Log("x: " + x + ", y: " + y);
            grid[x,y] = new Node(room, new Vector2(x,y), centerPos, tiles[i]);
            x++;
        }
        return result;
    }

    private void initGridGraph()
    {
        for(int x=0; x<grid.GetUpperBound(0); x++)
        {
            for(int y=0; y<grid.GetUpperBound(1); y++)
            {
                var upIndex = y + 1;
                var downIndex = y - 1;
                var rightIndex = x + 1;
                var leftIndex = x - 1;
                if(upIndex >= 0 && upIndex < grid.GetUpperBound(1))
                    grid[x,y].Up = grid[x, upIndex];
                if(downIndex >= 0 && downIndex < grid.GetUpperBound(1))
                    grid[x,y].Down = grid[x, downIndex];
                if(rightIndex >= 0 && rightIndex < grid.GetUpperBound(0))
                    grid[x,y].Right = grid[rightIndex, y];
                if(leftIndex >= 0 && leftIndex < grid.GetUpperBound(0))
                    grid[x,y].Left = grid[leftIndex, y];
            }
        }
    }

    private void SetupTiles()
    {
         
    }
}
