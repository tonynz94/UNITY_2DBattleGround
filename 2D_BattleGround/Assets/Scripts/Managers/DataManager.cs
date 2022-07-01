using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

[Serializable]


public class DataManager
{
    public Dictionary<int, Data.Stat> _statData { get; private set; } = new Dictionary<int, Data.Stat>();
    public void Init()
    {
        _statData = LoadJson<Data.StatData, int, Data.Stat>("StatData").MakeDict();
    }

    //반환하는 클래스명, 
    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }


}
