using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    class Room
    {
        object _lock = new object();
        public Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        public void Broadcast(ArraySegment<byte> packet)
        {
        	//jobQueue를 사용시
        	//_pendingList.Add(segment);		
        	lock(_lock)
        	{
        		foreach(Player player in _playerDic.Values)
        		{
        			player.Session.Send(packet);
        		}
        	}
        }
    }

    class GameRoom : Room
    {
        public int roomOwner;   //CGUID
        public int roomId;
        public Define.MapType _mapType;
        public Define.GameMode _gameMode;


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

    class LobbyRoom : Room
    {
        public void EnterLobbyRoom(int CGUID)
        {
            _playerDic.Add(CGUID, PlayerManager.Instance.GetPlayer(CGUID));
        }

        public void LeaveLobbyRoom(int CGUID)
        {
            _playerDic.Remove(CGUID);
        }

        public void HandleChatting(C_SendChat cPkt)
        {
            S_SendChat sPkt = new S_SendChat();
            sPkt.messageType = (int)Define.ChatType.Channel;
            sPkt.nickName = cPkt.nickName;
            sPkt.chatContent = cPkt.chatContent;

            Broadcast(sPkt.Write());
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
        public LobbyRoom GetLobby()
        {
            return _lobbyRoom;
        }
        public GameRoom GetGameRoom(int roomId)
        {
            GameRoom gameRoom;
            _gameRooms.TryGetValue(roomId, out gameRoom);
            return gameRoom;
        }


        public void MoveIntroToLobbyRoom(int CGUID)
        {
            lock (_lock)
            {
                _lobbyRoom.EnterLobbyRoom(CGUID);
            }
        }

        public void MoveLobbytToGameRoom(int CGUID, int roomId)
        {
            lock (_lock)
            {
                _lobbyRoom.LeaveLobbyRoom(CGUID);
                GameRoom gameRoom = GetGameRoom(roomId);
                gameRoom.AddPlayer(CGUID);
            }
        }

        public void MoveGameToLobbyRoom(int CGUID, int roomId)
        {
            lock (_lock)
            {
                GameRoom gameRoom = GetGameRoom(roomId);
                gameRoom.LeaveGameRoom(CGUID);
                _lobbyRoom.EnterLobbyRoom(CGUID);
            }
        }

        public void CreateGameRoom(GameRoom gameRoom)
        {
            lock (_lock)
            {
                gameRoom.roomId = _roomId;
                _gameRooms.Add(_roomId, gameRoom);
                _roomId++;
            }
        }

        public void RemoveGameRoom(int roomId)
        {
            _gameRooms.Remove(roomId);
        }


        public void RoomAllClear()
        {
            _gameRooms.Clear();
        }
    }
}
