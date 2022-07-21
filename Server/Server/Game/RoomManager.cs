using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    class Room
    {
        object _lock = new object();
        public Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        public int GetPlayerCount()
        {
            return _playerDic.Count;
        }

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


        public void MoveIntroToLobbyRoom(ClientSession session ,int CGUID)
        {
            lock (_lock)
            {
                _lobbyRoom.EnterLobbyRoom(CGUID);
                //to me
                //Send All Player Online in the game

                //other who already in Game
                //Send Only me to Add

                S_AllPlayerList sPkt1 = new S_AllPlayerList();
                foreach(Player player in PlayerManager.Instance._players.Values)
                {
                    S_AllPlayerList.OnLinePlayer temp = new S_AllPlayerList.OnLinePlayer();
                    temp.CGUID = player.Session.SessionId;
                    temp.Level = player.Info.Level;
                    temp.playerNickName = player.Info.NickName;

                    sPkt1.onLinePlayers.Add(temp);

                    session.Send(sPkt1.Write());
                }

                S_FirstEnter sPkt2 = new S_FirstEnter();
                sPkt2.CGUID = CGUID;
                sPkt2.playerNickName = PlayerManager.Instance.GetPlayerNickName(CGUID);

                PlayerManager.Instance.BroadCast(sPkt2.Write());
            }
        }

        public void MoveLobbytToGameRoom(int CGUID, int roomId)
        {
            lock (_lock)
            {
                bool isNotSlot = false;
                _lobbyRoom.LeaveLobbyRoom(CGUID);
                GameRoom gameRoom = GetGameRoom(roomId);

                if(gameRoom.GetPlayerCount() < 4)
                {
                    gameRoom.AddPlayer(CGUID);
                    isNotSlot = false;
                }
                else
                {
                    isNotSlot = true;
                }
                

                S_LobbyToGame sPkt = new S_LobbyToGame();
                sPkt.CGUID = CGUID;
                sPkt.IsNoSlot = isNotSlot;
                gameRoom.Broadcast(sPkt.Write());
            }
        }

        public void MoveGameToLobbyRoom(int CGUID, int roomId)
        {
            lock (_lock)
            {
                GameRoom gameRoom = GetGameRoom(roomId);
                gameRoom.LeaveGameRoom(CGUID);
                if (gameRoom.GetPlayerCount() == 0)
                    RemoveGameRoom(roomId);

                C_LobbyToGame cPkt = new C_LobbyToGame();
                cPkt.CGUID = CGUID;
                gameRoom.Broadcast(cPkt.Write());

                _lobbyRoom.EnterLobbyRoom(CGUID);

            
            }
        }

        public void HandleCreateGameRoom(C_CreateGameRoom cPkt)
        {
            lock (_lock)
            {
                GameRoom gameRoom = new GameRoom();
                gameRoom.roomOwner = cPkt.CGUID;
                gameRoom.AddPlayer(cPkt.CGUID);
                gameRoom.roomId = _roomId++;
                gameRoom.SetGameRoom((Define.GameMode)cPkt.GameType , (Define.MapType)cPkt.MapType);
                _gameRooms.Add(gameRoom.roomId, gameRoom);

                //반들어 준 사람에게 에코로 보내 줌
                S_CreateGameRoom sPkt = new S_CreateGameRoom();
                sPkt.CGUID = gameRoom.roomOwner;
                sPkt.RoomId = gameRoom.roomId;
                sPkt.MapType = (int)gameRoom._mapType;
                sPkt.GameType = (int)gameRoom._gameMode;
             
                ClientSession session = SessionManager.Instance.Find(sPkt.CGUID);
                session.Send(sPkt.Write());
            }
        }

        public void HandleGetAllGameRooms(ClientSession  session)
        {
            lock(_lock)
            {
                S_GetGameRooms sPkt = new S_GetGameRooms();
                
                foreach (KeyValuePair<int, GameRoom> gameRoom  in _gameRooms)
                {
                    S_GetGameRooms.GameRoomlist tempRoom = new S_GetGameRooms.GameRoomlist();
                    tempRoom.GameMode = (int)gameRoom.Value._gameMode;
                    tempRoom.MapType = (int)gameRoom.Value._mapType;
                    tempRoom.RoomId = gameRoom.Key;
                    tempRoom.RoomOwner = gameRoom.Value.roomOwner;
                    foreach (KeyValuePair<int, Player> player in gameRoom.Value._playerDic)
                    {
                        S_GetGameRooms.GameRoomlist.PlayerList tempPlayer = new S_GetGameRooms.GameRoomlist.PlayerList();
                        tempPlayer.CGUID = player.Key;
                        tempRoom.playerLists.Add(tempPlayer);
                    }

                    sPkt.gameRoomlists.Add(tempRoom);
                }

                session.Send(sPkt.Write());
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
