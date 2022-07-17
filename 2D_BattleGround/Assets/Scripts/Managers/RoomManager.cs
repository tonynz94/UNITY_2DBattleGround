using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyRoom
{
    public int roomOwner;   //CGUID
    public Dictionary<int, Player> playerDic = new Dictionary<int, Player>();
    
}

//로비 방 모음
public class RoomManager : MonoBehaviour
{
    public Dictionary<int, LobbyRoom> lobbyRooms { get; set; } = new Dictionary<int, LobbyRoom>();

    public void RoomClear()
    {
        lobbyRooms.Clear();
    }
}
