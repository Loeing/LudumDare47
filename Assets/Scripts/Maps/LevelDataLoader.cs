using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class LevelDataLoader : MonoBehaviour {

    private const string LevelsPath = "Levels";
    public Dictionary<int, LevelsData.LevelData> LoadLevelsData()
    {
        var jsonFile = Resources.Load(LevelsPath, typeof(TextAsset)) as TextAsset;
        if (jsonFile == null)
        {
            throw new SystemException("Levels file is not accessible");
        }
        var loadedData = JsonUtility.FromJson<LevelsData>(jsonFile.text);
        Debug.Log(jsonFile.text);
        Debug.Log(loadedData.levels[0].number);
        Debug.Log(loadedData.levels[0].tiles);
        return loadedData.levels.ToDictionary(level => level.number, level => level);
    }    
}