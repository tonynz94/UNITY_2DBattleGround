using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    Dictionary<int, GameObject> _playerDic = new Dictionary<int, GameObject>();

    Dictionary<int, GameObject> _waterBoomObjectDic = new Dictionary<int, GameObject>();
    LinkedList<GameObject> _itemObjectList = new LinkedList<GameObject>();

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

    public void HandleMove(S_BroadcastMove sPkt)
    {
        GameObject go = GetPlayerObject(sPkt.CGUID);
        BaseController controller = go.GetComponent<BaseController>();

        controller.CellPos = new Vector3Int(sPkt.cellPosX, sPkt.cellPosY, 0);
        controller.DestPos = new Vector3(sPkt.posX, sPkt.posY, 0);
        controller.Dir = (Define.MoveDir)sPkt.Dir;
        controller.State = (Define.ObjectState)sPkt.State;
    }

    public GameObject FindObjectsInField(Vector2Int cellPos)
    {
        foreach (GameObject obj in _waterBoomObjectDic.Values)
        {
            WaterBoomObject boom = obj.GetComponent<WaterBoomObject>();
            if (boom == null)
                continue;

            if (boom.GetPos() == cellPos)
                return obj;
        }

        //foreach (GameObject obj in _itemObejctList)
        //{
        //    BaseController player = obj.GetComponent<BaseController>();
        //}

        return null;
    }

    public GameObject FindPlayersInField(Vector2Int cellPos)
    {
        foreach (GameObject obj in _playerDic.Values)
        {
            BaseController player = obj.GetComponent<BaseController>();
            if (player.CellPos == new Vector3Int(cellPos.x, cellPos.y))
                return obj;
        }

        return null;
    }

    public void SetWaterBoomInField(S_WaterBOOM sPkt)
    {
        int cellPosX = sPkt.CellPosX;
        int cellPosY = sPkt.CellPosY;

        Vector2Int cellPos = new Vector2Int(cellPosX, cellPosY);
        GameObject boomObject = Managers.Resource.Instantiate("Objects/WaterBoomObject");

        boomObject.GetComponent<WaterBoomObject>().SetInField(cellPos);
        _waterBoomObjectDic.Add(sPkt.ID, boomObject);


        boomObject.transform.localPosition = new Vector3(cellPosX + 0.5f, cellPosY + 0.5f, 0);
    }

    public void BlowWaterBoom(S_WaterBlowUp sPkt)
    {
        GameObject waterBoom = null;
        _waterBoomObjectDic.TryGetValue(sPkt.ID, out waterBoom);
        _waterBoomObjectDic.Remove(sPkt.ID);

        waterBoom.GetComponent<WaterBoomObject>().WaterBoomBlowUp();
    }

    public void LeaveGame(int CGUID)
    {
        Managers.Resource.Destroy(GetPlayerObject(CGUID));
        _playerDic.Remove(CGUID); 
    }

    public void ClearAll()
    {
        _playerDic.Clear();
        //_monsterList.Clear();
    }
}
