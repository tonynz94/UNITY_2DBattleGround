using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoom
{
    public int roomOwner;   //CGUID
    public int roomId;
    public Define.MapType _mapType;
    public Define.GameMode _gameMode;
    public Dictionary<int, Player> playerDic = new Dictionary<int, Player>();

    public void AddPlayer(int CGUID)
    {
        playerDic.Add(CGUID, Managers.Player.GetPlayer(CGUID));
    }

    public void LeaveGameRoom(int CGUID)
    {
        playerDic.Remove(CGUID);
    }

    public void SetGameRoom(Define.GameMode gameMode, Define.MapType mapType)
    {
        _gameMode = gameMode;
        _mapType = mapType;
    }

    public void Clear()
    {
        _mapType = Define.MapType.None;
        _gameMode = Define.GameMode.None;
    }
}

public class LobbyRoom
{
    public Dictionary<int, Player> playerDic = new Dictionary<int, Player>();

    public void EnterLobbyRoom(int CGUID)
    {
        playerDic.Add(CGUID, Managers.Player.GetPlayer(CGUID));
    }

    public void LeaveLobbyRoom(int CGUID)
    {
        playerDic.Remove(CGUID);
    }

}

//로비 방 모음
public class RoomManager : MonoBehaviour
{
    public Dictionary<int, GameRoom> _gameRooms { get; private set; } = new Dictionary<int, GameRoom>();
    public LobbyRoom _lobbyRoom { get; private set; } = new LobbyRoom();

    public int _roomId = 10000;

    public void MoveIntroToLobbyRoom(int CGUID)
    {
        _lobbyRoom.EnterLobbyRoom(CGUID);
    }

    public void MoveLobbytToGameRoom(int CGUID, int roomId)
    {
        _lobbyRoom.LeaveLobbyRoom(CGUID);
        GameRoom gameRoom = GetGameRoom(roomId);
        gameRoom.AddPlayer(CGUID);
    }

    public void MoveGameToLobbyRoom(int CGUID, int roomId)
    {
        GameRoom gameRoom = GetGameRoom(roomId);
        gameRoom.LeaveGameRoom(CGUID);
        _lobbyRoom.EnterLobbyRoom(CGUID);
    }

    public void CreateGameRoom(GameRoom gameRoom)
    {
        Debug.Log("Create Room Complete");
        gameRoom.roomId = _roomId;
        _gameRooms.Add(_roomId, gameRoom);
        _roomId++;
    }

    public void RemoveGameRoom(int roomId)
    {
        _gameRooms.Remove(roomId);
    }

    public GameRoom GetGameRoom(int roomId)
    {
        GameRoom gameRoom;
        _gameRooms.TryGetValue(roomId, out gameRoom);
        return gameRoom;
    }

    public void RoomAllClear()
    {
        _gameRooms.Clear();
    }
}
