using System;
using UnityEngine;

[Serializable]
public class LevelsData
{
    public LevelData[] levels;
    
    [Serializable]
    public class LevelData
    {
        public int number;
        public int[] tiles;
        public int rowLength;
        public EntityData[] entities;
        public PassageData[] passages;
    }

    [Serializable]
    public class EntityData
    {
        public string name;
        public int x;
        public int y;
        
    }

    [Serializable]
    public class PassageData
    {
        public int srcX;
        public int srcY;
        public int destRoom;
        public int destX;
        public int destY;
    } 
}