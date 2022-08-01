using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoom
{
    public int roomOwner;   //CGUID
    public int roomId;
    public Define.MapType _mapType;
    public Define.GameMode _gameMode;
    public List<Player> _playerList = new List<Player>();

    public void AddPlayer(int CGUID)
    {
        _playerList.Add(Managers.Player.GetPlayer(CGUID));
    }
    public void AddPlayer(Player player)
    {
        _playerList.Add(player);
    }

    public void LeaveGameRoom(int leavePlayerCGUID)
    {
        Player player = _playerList.Find(x => x._CGUID == leavePlayerCGUID);
        if (player == null)
        {
            Debug.LogError("Theres no such player in this Room");
            return;
        }
        else
        {
            player.LeaveFromGameRoom();
            _playerList.Remove(player);
        }
    }

    public void SetNewOwner(int newOwnerCGUID)
    {
        roomOwner = newOwnerCGUID;

        Player player = _playerList.Find(x => x._CGUID == newOwnerCGUID);
        player.IsPlayerReady = false;
    }

    public void SetGameRoom(Define.GameMode gameMode, Define.MapType mapType)
    {
        _gameMode = gameMode;
        _mapType = mapType;
    }

    public void GameStart()
    {
        //로딩화면 나오게 하면 좋음.
        Managers.Scene.ChangeScene(Define.Scene.GameScene);
    }

    public int GetPlayerCount()
    {
        return _playerList.Count;
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

    public void HandleLobbyToGameRoom(S_LobbyToGame sPkt)
    {
        if (sPkt.IsPlayerEntered)
        {
            GameRoom room = Managers.Room.GetGameRoom(sPkt.roomId);

            //만약 내가 로비에서 새로운 방에 들어간거라면
            if (Managers.Player.GetMyCGUID() == sPkt.CGUID)
            {
                Managers.UI.ClosePopupUI();
                UI_GameRoom roomScript = Managers.UI.ShowPopupUI<UI_GameRoom>();
                roomScript.EnterRoom(sPkt.roomId, sPkt.CGUID);
            }
            //만약 내가 속한 방에 한 유저가 입장했을 때
            else
            {
                UI_GameRoom roomScript = Managers.UI.PeekPopupUI<UI_GameRoom>();
                roomScript.EnterRoom(sPkt.roomId, sPkt.CGUID);
            }
        }
        else
        {
            //방 꽉찼다고 팝업창 뛰우기.
            Debug.Log("popup : This Room is Full");
        }
    }

    //누군가 나갔을 때 (나 일수도있고 방안에 다른 플레이어 일 수도 있음.)
    public void HandleGameToLobby(S_GameToLobby sPkt)
    {
        GameRoom room = Managers.Room.GetGameRoom(sPkt.roomId);
        int leavePlayerCGUID = sPkt.CGUID;

        room.LeaveGameRoom(leavePlayerCGUID);

        //내가 나갔을때.
        if (leavePlayerCGUID == Managers.Player.GetMyCGUID())
        {  
            Managers.UI.ClosePopupUI();
            Managers.Room.GameRoomAllClear();   
        }
        //누군가 나갔을때.
        else
        {
            //나간사람이 방장이면. 
            if (room.roomOwner == leavePlayerCGUID)
                room.SetNewOwner(sPkt.newOwner);
                
            UI_GameRoom roomScript = Managers.UI.PeekPopupUI<UI_GameRoom>();

            roomScript.RefreashSlot();
            roomScript.RefreashTitle();
        }
    }

    public void HandleGetAllGameRooms(S_GetGameRooms sPkt)
    {
        foreach(S_GetGameRooms.GameRoomlist temp in sPkt.gameRoomlists)
        {
            GameRoom gameRoom = new GameRoom();
            gameRoom.SetGameRoom((Define.GameMode)temp.GameMode, (Define.MapType)temp.MapType);
            gameRoom.roomId = temp.RoomId;
            gameRoom.roomOwner = temp.RoomOwner;
            
            foreach(S_GetGameRooms.GameRoomlist.PlayerList tempPlayer in temp.playerLists)
            {
                Player player = Managers.Player.GetPlayer(tempPlayer.CGUID);

                player.IsPlayerReady = tempPlayer.isReady;
                gameRoom.AddPlayer(player);
            }
            _gameRooms.Add(gameRoom.roomId, gameRoom);
        }

        UI_RoomList roomlist = Managers.UI.ShowPopupUI<UI_RoomList>();
        roomlist.RefreshGameRoom();
    }

    public void HandleGameStart(S_GameStart sPkt)
    {
        if(sPkt.isStart == true)
        {
            GetGameRoom(sPkt.roomID).GameStart();
        }
        else
        {
            Debug.Log("popup : Everyone has to be Ready");
        }
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
