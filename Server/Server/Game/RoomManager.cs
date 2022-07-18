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
        public Dictionary<int, Player> playerDic = new Dictionary<int, Player>();

        public void AddPlayer(int CGUID)
        {
            playerDic.Add(CGUID, PlayerManager.Instance.GetPlayer(CGUID));
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


    class RoomManager
    {
        public static RoomManager Instance { get; } = new RoomManager();

        Dictionary<int, GameRoom> _gameRooms = new Dictionary<int, GameRoom>();
        LobbyRoom _lobbyRoom = new LobbyRoom();

        object _lock = new object();
        int roomId = 1;

        //실행 중인 게임 추가
        public void FirstEnterToLobby(Player player)
        {
            lock (_lock)
            {
                _lobbyRoom.EnterLobby(player);

               
            }
        }

        public GameRoom Add()
        {
            GameRoom gameRoom = new GameRoom();
            lock(_lock)
            {
                gameRoom.RoomID = roomId++;
                _gameRooms.Add(gameRoom.RoomID, gameRoom);
            }
            return gameRoom;
        }

        //끝난 게임 삭제
        public GameRoom Remove(int roomId)
        {
            return null;
        }

        public GameRoom Find(int roomId)
        {
            if(roomId == 0)
            {
                return _lobbyRoom;
            }
            else
            {
                GameRoom gameRoom;
                _gameRooms.TryGetValue(roomId, out gameRoom);
                if (gameRoom != null)
                    return gameRoom;
            }

            Console.WriteLine($"theres no such a room {roomId}");
            return null;
        }
    }
}
