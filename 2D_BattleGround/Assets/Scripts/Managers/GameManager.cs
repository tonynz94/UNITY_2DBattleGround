using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    //게임내에 있는 모든 움직이는 오브젝트들
    HashSet<GameObject> _playerList = new HashSet<GameObject>();
    HashSet<GameObject> _monsterList = new HashSet<GameObject>();

    //게임에 입장한다.
    public void EnterGame()
    {

    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();

        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public GameObject Spawn(Define.WorldObject type, string path, Transform parent = null)
    {
        GameObject go = Managers.Resource.Instantiate(path, parent);
        switch(type)
        {
            case Define.WorldObject.Player:
                _playerList.Add(go);
                break;
            case Define.WorldObject.Monster:
                _monsterList.Add(go);
                break;
            case Define.WorldObject.Boss:
                break;
        }
        return go;
    }

    public void Despawn(GameObject go)
    {
       // if(go)
    }

    public int GetPlayerCount()
    {
        return _playerList.Count;
    }

    public int GetMonsterCount()
    {
        return _monsterList.Count;
    }

    public void LeaveGame(GameObject player)
    {
        _playerList.Remove(player);
        Managers.Resource.Destroy(player);
    }

    //public GameObject FindPlayerByPos(Vector3Int cellPos)
    //{
    //    foreach(GameObject go in _playerList)
    //    {
            
    //    }
    //}

    public void ClearAll()
    {
        _playerList.Clear();
    }
}
