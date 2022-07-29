using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoom
{
    public int roomOwner;   //CGUID
    public int roomId;
    public Define.MapType _mapType;
    public Define.GameMode _gameMode;
    public Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

    public void AddPlayer(int CGUID)
    {
        _playerDic.Add(CGUID, Managers.Player.GetPlayer(CGUID));
    }

    public void LeaveGameRoom(int CGUID)
    {
        _playerDic.Remove(CGUID);
    }

    public void SetGameRoom(Define.GameMode gameMode, Define.MapType mapType)
    {
        _gameMode = gameMode;
        _mapType = mapType;
    }

    public int GetPlayerCount()
    {
        return _playerDic.Count;
    }

    public void Clear()
    {
        _mapType = Define.MapType.None;
        _gameMode = Define.GameMode.None;
    }
}

public class LobbyRoom
{
    public Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

    public void EnterLobbyRoom(int CGUID)
    {
        _playerDic.Add(CGUID, Managers.Player.GetPlayer(CGUID));
    }

    public void LeaveLobbyRoom(int CGUID)
    {
        _playerDic.Remove(CGUID);
    }

}

public class RoomManager
{
    public Dictionary<int, GameRoom> _gameRooms { get; private set; } = new Dictionary<int, GameRoom>();
    public LobbyRoom _lobbyRoom { get; private set; } = new LobbyRoom();

    //public void MoveIntroToLobbyRoom(int CGUID)
    //{
    //    _lobbyRoom.EnterLobbyRoom(CGUID);
    //}

    //public void MoveLobbytToGameRoom(int CGUID, int roomId)
    //{
    //    _lobbyRoom.LeaveLobbyRoom(CGUID);
    //    GameRoom gameRoom = GetGameRoom(roomId);
    //    gameRoom.AddPlayer(CGUID);
    //}

    //public void MoveGameToLobbyRoom(int CGUID, int roomId)
    //{
    //    GameRoom gameRoom = GetGameRoom(roomId);
    //    gameRoom.LeaveGameRoom(CGUID);
    //    _lobbyRoom.EnterLobbyRoom(CGUID);
    //}

    public void HandleCreateGameRoom(S_CreateGameRoom sPkt)
    {
        Debug.Log("Create Room Complete");
        
        GameRoom gameRoom = new GameRoom();
        gameRoom.roomId = sPkt.RoomId;
        gameRoom.roomOwner = sPkt.CGUID;
        gameRoom.SetGameRoom((Define.GameMode)sPkt.GameType, (Define.MapType)sPkt.MapType);
        _gameRooms.Add(gameRoom.roomId, gameRoom);

        Managers.UI.ClosePopupUI();
        UI_GameRoom room = Managers.UI.ShowPopupUI<UI_GameRoom>();

        //Managers.Room.GetGameRoom(gameRoom.roomId).roomOwner
        room.CreateRoom(gameRoom.roomId, Managers.Player.MyPlayer);
    }

    public void HandleLobbyToGameRoom(S_LobbyToGame rPkt)
    {
        GameRoom room = Managers.Room.GetGameRoom(rPkt.roomId);

        //만약 내가 로비에서 새로운 방에 들어간거라면
        if (Managers.Player.GetMyCGUID() == rPkt.CGUID)
        {
            Managers.UI.ClosePopupUI();
            UI_GameRoom roomScript = Managers.UI.ShowPopupUI<UI_GameRoom>();
            roomScript.EnterRoom(rPkt.roomId, rPkt.CGUID);
        }
        //만약 내가 속한 방에 한 유저가 입장했을 때
        else
        {
            UI_GameRoom roomScript = Managers.UI.PeekPopupUI<UI_GameRoom>();
            roomScript.EnterRoom(rPkt.roomId, rPkt.CGUID);
        }

    }

    public void HandleGetAllGameRooms(S_GetGameRooms sPkt)
    {
        foreach(S_GetGameRooms.GameRoomlist temp in sPkt.gameRoomlists)
        {
            GameRoom gameRoom = new GameRoom();
            gameRoom.SetGameRoom((Define.GameMode)temp.GameMode, (Define.MapType)temp.MapType);
            gameRoom.roomId =temp.RoomId;
            gameRoom.roomOwner =temp.RoomOwner;
            
            foreach(S_GetGameRooms.GameRoomlist.PlayerList tempPlayer in temp.playerLists)
            {
                gameRoom.AddPlayer(tempPlayer.CGUID);
            }
            _gameRooms.Add(gameRoom.roomId, gameRoom);
        }

        UI_RoomList roomlist = Managers.UI.ShowPopupUI<UI_RoomList>();
        roomlist.RefreshGameRoom();
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

    public void GameRoomAllClear()
    {
        _gameRooms.Clear();
    }
}
