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
    private Dictionary<int, TileBase> tiles = new Dictionary<int, TileBase>();
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
    void Start()
    {
        _levelsData = _dataLoader.LoadLevelsData();  
        Debug.Log(_levelsData.Count + " levels have been stored in the dictionary!");
        var baseTilemap = GetComponentsInChildren<Tilemap>()[0];
        Debug.Log(_levelsData);
        Debug.Log(_levelsData[1]);
        Debug.Log(_levelsData[1].number);
        Debug.Log(_levelsData[1].tiles);
        var mapData = ListToMap(_levelsData[1].tiles, _levelsData[1].rowLength);
        MapFunctions.RenderMap(mapData, baseTilemap, tiles);
    }

    private int[,] ListToMap(int[] tiles, int rowLength)
    {
        Debug.Log(tiles.Length);
        Debug.Log(rowLength);
        Debug.Log(tiles.Length/rowLength);
        int rows = (int)Math.Ceiling((double) tiles.Length/(double)rowLength);
        Debug.Log(rows);
        int[,] result = new int[rows, rowLength];
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
            y++;
        }
        return result;
    }

    private void SetupTiles()
    {
         
    }
}
