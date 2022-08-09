using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    Dictionary<int, GameObject> _playerDic = new Dictionary<int, GameObject>();
    //<GameObject> _monsterList = new HashSet<GameObject>();

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

    public void SpawnWorldObject(Define.WorldObject type, int CGUID, Vector3 spawnPos ,Transform parent = null)
    {
        GameObject go = null;

        bool isMyPlayer = Managers.Player.GetMyCGUID() == CGUID;

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
                _playerDic.Add(CGUID, go);
                break;
            case Define.WorldObject.Monster:
                //_monsterList.Add(go);
                break;
            case Define.WorldObject.Boss:
                break;
        }
    }

    public void Despawn(GameObject go)
    {
       
    }

    public GameObject GetPlayerObject(int CGUID)
    {
        GameObject playerObject;
        _playerDic.TryGetValue(CGUID, out playerObject);

        return playerObject;
    }

    public int GetPlayerCount()
    {
        return _playerDic.Count;
    }

    //public int GetMonsterCount()
    //{
    //    return _monsterList.Count;
    //}

    public void HandleMove(S_BroadcastMove sPkt)
    {
        GameObject go = GetPlayerObject(sPkt.CGUID);
        BaseController controller = go.GetComponent<BaseController>();

        controller.CellPos = new Vector3Int(sPkt.cellPosX, sPkt.cellPosY, 0);
        controller.DestPos = new Vector3(sPkt.posX, sPkt.posY, 0);
        controller.Dir = (Define.MoveDir)sPkt.Dir;
        controller.State = (Define.ObjectState)sPkt.State;
    }

    public void SetWaterBOOMInGameField(S_WaterBOOM sPkt)
    {
        int cellPosX = sPkt.CellPosX;
        int cellPosY = sPkt.CellPosY;

        GameObject go = Managers.Resource.Instantiate("Objects/WaterBoomObject");
        go.transform.localPosition = new Vector3(cellPosX + 0.5f, cellPosY + 0.5f, 0);
    }

    public void LeaveGame(int CGUID)
    {
        _playerDic.Remove(CGUID);
        Managers.Resource.Destroy(GetPlayerObject(CGUID));
    }

    public void ClearAll()
    {
        _playerDic.Clear();
        //_monsterList.Clear();
    }
}
