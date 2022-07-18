using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameRoom
    {
        public int roomOwner;   //CGUID
        public int roomId;
        public Define.MapType _mapType;
        public Define.GameMode _gameMode;
        Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        public void AddPlayer(int CGUID)
        {
            _playerDic.Add(CGUID, PlayerManager.Instance.GetPlayer(CGUID));
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

        public void Clear()
        {
            _mapType = Define.MapType.None;
            _gameMode = Define.GameMode.None;
        }
    }

    public class LobbyRoom
    {
        Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        public void EnterLobbyRoom(int CGUID)
        {
            _playerDic.Add(CGUID, PlayerManager.Instance.GetPlayer(CGUID));
        }

        public void LeaveLobbyRoom(int CGUID)
        {
            _playerDic.Remove(CGUID);
        }
    }


    class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();

        Dictionary<int, GameRoom> _gameRooms = new Dictionary<int, GameRoom>();
        LobbyRoom _lobbyRoom = new LobbyRoom();

        object _lock = new object();
        int _roomId = 10000;

        //실행 중인 게임 추가
        public void MoveIntroToLobbyRoom(Player player)
        {
            lock (_lock)
            {
                _lobbyRoom.EnterLobbyRoom(player.Session.SessionId);
            }
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
}
