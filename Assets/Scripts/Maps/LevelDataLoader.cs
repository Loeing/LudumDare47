using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class LevelDataLoader : MonoBehaviour {

    private const string LevelsPath = "Levels";
    public GameObject roomObject;

    public Dictionary<int, Room> GenerateRooms()
    {
        Dictionary<int, Room> rooms = new Dictionary<int, Room>();
        foreach(KeyValuePair<int, LevelsData.LevelData> entry in LoadLevelsData())
        {
            rooms.Add(entry.Key,  LevelDataToRoom(entry.Value));
        }
        return rooms;
    }

    private Dictionary<int, LevelsData.LevelData> LoadLevelsData()
    {
        var jsonFile = Resources.Load(LevelsPath, typeof(TextAsset)) as TextAsset;
        if (jsonFile == null)
        {
            throw new SystemException("Levels file is not accessible");
        }
        var loadedData = JsonUtility.FromJson<LevelsData>(jsonFile.text);
        //Debug.Log(jsonFile.text);
        //Debug.Log(loadedData.levels[0].number);
        //Debug.Log(loadedData.levels[0].tiles);
        return loadedData.levels.ToDictionary(level => level.number, level => level);
    } 

    private Room LevelDataToRoom(LevelsData.LevelData levelData)
    {
        int[] tiles = levelData.tiles;
        int rowLength = levelData.rowLength;
        int rows = (int)Math.Ceiling((double) tiles.Length/(double)rowLength);
        int[,] tileGrid = new int[rows, rowLength];
        Node[,] nodeGrid = new Node[rows, rowLength];
        int x = 0;
        int y = tiles.Length/rowLength - 1;
        for(int i=0; i < tiles.Length; i++)
        {
            if(x >= rowLength)
            {
                x = 0;
                y--;
            }
            tileGrid[x,y] = tiles[i];
            Vector3 centerPos = new Vector3(x + 0.5f, y + 0.5f, 0);
            //Vector3Int cellPosition = baseTilemap.WorldToCell(new Vector3(x,y,0));
            //Vector3 centerPos = baseTilemap.GetCellCenterWorld(cellPosition);
            //Debug.Log("x: " + x + ", y: " + y);
            nodeGrid[x,y] = new Node(levelData.number, new Vector2(x,y), centerPos, tiles[i]);
            x++;
        }
        //Debug.Log("UGH");
        //Debug.Log(levelData.entities);
        //Debug.Log(nodeGrid);
        GameObject instance = Instantiate(roomObject);
        Room room = instance.GetComponent<Room>();
        return room.RoomInit(levelData.number, tileGrid, nodeGrid, levelData.entities, levelData.passages);
    }


}