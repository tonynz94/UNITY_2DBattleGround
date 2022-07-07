using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    //���ӳ��� �ִ� ��� �����̴� ������Ʈ��
    List<GameObject> _playerList = new List<GameObject>();

    //���ӿ� ������ 
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
