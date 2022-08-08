using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    HashSet<GameObject> _playerList = new HashSet<GameObject>();
    HashSet<GameObject> _monsterList = new HashSet<GameObject>();

    Define.MapType _mapType;
    Define.GameMode _gameMode;

    int _roomID;

    public int GetCurrentRoomID()
    {
        return _roomID;
    }
    
    public void GameStart(int roomID, Define.MapType mapType, Define.GameMode gameMode)
    {
        _roomID = roomID;
        _mapType = mapType;
        _gameMode = gameMode;

        ClearAll();
    }

    public Define.WorldObject GetWorldObjectType(GameObject go)
    {
        BaseController bc = go.GetComponent<BaseController>();

        if (bc == null)
            return Define.WorldObject.Unknown;

        return bc.WorldObjectType;
    }

    public void SpawnWorldObject(Define.WorldObject type, bool isMyPlayer, Vector3 spawnPos ,Transform parent = null)
    {
        GameObject go = null;
        if (isMyPlayer)
        {
            go = Managers.Resource.Instantiate("Objects/MyPlayer");
            Camera.main.GetComponent<CameraController>().TargetPoint(go);
        }
        else
        {
            go = Managers.Resource.Instantiate("Objects/Player");
        }

        //go.name = 
        //go.transform.localPosition = spawnPos;

        switch (type)
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
    }

    public void Despawn(GameObject go)
    {
       
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

    public void ClearAll()
    {
        _playerList.Clear();
        _monsterList.Clear();
    }
}
