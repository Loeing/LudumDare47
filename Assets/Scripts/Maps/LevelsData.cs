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
    }
}