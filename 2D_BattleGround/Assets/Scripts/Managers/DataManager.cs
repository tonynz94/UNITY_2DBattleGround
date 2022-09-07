using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataManager
{
    public Dictionary<int, Data.LevelStat> LevelStatDict { get; private set; } = new Dictionary<int, Data.LevelStat>();
    public void Init()
    {
        LevelStatDict = LoadJson<LevelStatData, int, Data.LevelStat>("LevelStat").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path)
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
