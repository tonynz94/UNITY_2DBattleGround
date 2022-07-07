using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    //게임내에 있는 모든 움직이는 오브젝트들
    List<GameObject> _playerList = new List<GameObject>();

    //게임에 입장한 
    public void EnterGame(GameObject player)
    {
        _playerList.Add(player);
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
