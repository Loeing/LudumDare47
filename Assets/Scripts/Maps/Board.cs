using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
//using Math;


public class Board : MonoBehaviour
{

    [SerializeField]
    private TileBase floor;
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
    private Node[,] grid;
    

    private void Awake() 
    {
        _dataLoader = GetComponent<LevelDataLoader>();    
        tiles.Add(1, floor);
        tiles.Add(2, obstacle);
    }

    // Start is called before the first frame update
    public void InitGame(int level)
    {
        _levelsData = _dataLoader.LoadLevelsData();  
        Debug.Log(_levelsData.Count + " levels have been stored in the dictionary!");
        baseTilemap = GetComponentsInChildren<Tilemap>()[0];
        // Debug.Log(_levelsData);
        // Debug.Log(_levelsData[1]);
        // Debug.Log(_levelsData[1].number);
        // Debug.Log(_levelsData[1].tiles);
        var mapData = ListToMap(_levelsData[level].tiles, _levelsData[level].rowLength);
        MapFunctions.RenderMap(mapData, baseTilemap, tiles);
        initGridGraph();
        Vector3Int cellPosition = baseTilemap.WorldToCell(transform.position);
        Vector3 startPos = baseTilemap.GetCellCenterWorld(cellPosition);
        startPos.z = -1;
        //Vector3 startPos = new Vector3Int(0,0,0);
        GameObject pc = Instantiate(playerCharacter, startPos, Quaternion.identity);
        pc.GetComponent<PlayerController>().currentNode = grid[0,0];

        GameObject enemyOne = Instantiate(enemyCharacter, new Vector3(5.5f,5.5f,-1f), Quaternion.identity);
        enemyOne.GetComponent<EnemyController>().currentNode = grid[6,6];
        // Debug.Log("Starting Node: " + grid[0,0].gridPos);
        // Debug.Log("Starting Up: " + grid[0,0].Up.gridPos);
        // Debug.Log("Starting Down: " + grid[0,0].Down);
        // Debug.Log("Starting Left: " + grid[0,0].Left);
        // Debug.Log("Starting Right: " + grid[0,0].Right.gridPos);
    }

    public void PlaceInstance(GameObject go, int x, int y, int z=-1)
    {
        Node placementNode = grid[x, y];
        Vector3 placement = new Vector3(placementNode.center.x , placementNode.center.y, z);
        GameObject instance = Instantiate(go, placement, Quaternion.identity);
    }

    private int[,] ListToMap(int[] tiles, int rowLength)
    {
        //Debug.Log(tiles.Length);
        //Debug.Log(rowLength);
        //Debug.Log(tiles.Length/rowLength);
        int rows = (int)Math.Ceiling((double) tiles.Length/(double)rowLength);
        //Debug.Log(rows);
        int[,] result = new int[rows, rowLength];
        grid = new Node[rows, rowLength];
        int x = 0;
        int y = 0;
        for(int i=0; i < tiles.Length; i++)
        {
            if(y >= rowLength)
            {
                y = 0;
                x++;
            }
            result[x,y] = tiles[i];
            Vector3Int cellPosition = baseTilemap.WorldToCell(new Vector3(x,y,0));
            Vector3 centerPos = baseTilemap.GetCellCenterWorld(cellPosition);
            //Debug.Log("x: " + x + ", y: " + y);
            grid[x,y] = new Node(new Vector2(x,y), centerPos, tiles[i]);
            y++;
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
                    // Debug.Log("Up from " + x + ", " + y);
                    // Debug.Log("Is " + x + ", " + upIndex);
                    // Debug.Log("Game Coords: " + grid[x,y].gridPos);
                    // Debug.Log("Up Coords: " + grid[upIndex,y].gridPos);
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
