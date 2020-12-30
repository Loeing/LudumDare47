using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class MapFunctions
{
    public static void RenderMap(int[,] map, Tilemap tilemap, Dictionary<int, TileBase> tiles, int z = 0)
    {
        tilemap.ClearAllTiles();
        //Debug.Log(map);
        for(int x = 0; x <= map.GetUpperBound(0); x++)
        {
            for(int y = 0; y <= map.GetUpperBound(1); y++)
            {
                tilemap.SetTile(new Vector3Int(x,y,z), tiles[map[x,y]]);
            }
        }
    }
}