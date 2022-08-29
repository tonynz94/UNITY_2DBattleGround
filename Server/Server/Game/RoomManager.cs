using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{

    class GameRoom
    {
        object _lock = new object();

        public int roomOwner;   //CGUID
        public int roomId;
        public int _readyCnt = 0;
        public bool _isStarted = false;
        public Define.MapType _mapType;
        public Define.GameMode _gameMode;

        public List<Player> _playerList = new List<Player>();

        public int GetPlayerCount()
        {
            return _playerList.Count;
        }

        public void Broadcast(ArraySegment<byte> packet)
        {
            //jobQueue를 사용시
            //_pendingList.Add(segment);		
            lock (_lock)
            {
                foreach (Player player in _playerList)
                {
                    player.Session.Send(packet);
                }
            }
        }

        //처음 들어올때(방읆 만든사람도 이 함수를 통해서 들어옴)
        public void EnterGameRoom(int CGUID)
        {
            _playerList.Add(PlayerManager.Instance.GetPlayer(CGUID));
        }

        public void LeaveGameRoom(int CGUID)
        {
            Player player = _playerList.Find(x => x.Session.SessionId == CGUID);
            player.LeaveFromGameRoom();
            _playerList.Remove(player);
        }

        public void SetGameRoom(Define.GameMode gameMode, Define.MapType mapType)
        {
            _gameMode = gameMode;
            _mapType = mapType;
        }

        public int SetNewOwner()
        {
            Random rand = new Random();
            int ownerIndex = rand.Next(0, GetPlayerCount());

            //방장이 되었기 때문에 Ready를 False로 바꿔준다.
            roomOwner = _playerList[ownerIndex].Session.SessionId;
            _playerList[ownerIndex].Info.isPlayerReady = false;

            return _playerList[ownerIndex].Session.SessionId;
        }

        public void SetReady(bool isReady)
        {
            if (isReady)
                _readyCnt += 1;
            else
                _readyCnt -= 1;

            if (_readyCnt < 0 || _readyCnt > 4)
                Console.WriteLine("Ready < 0 or Ready > 4 it can't be");

        }

        public bool GameStart()
        {
            int cnt = 0;

            foreach(var player in _playerList)
            {
                if (player.Info.isPlayerReady)
                    cnt++;
            }

            if(GetPlayerCount() - 1 == cnt)
            {
                _isStarted = true;
                GameManager.Instance.NewGameStart(roomId);
                return true;
            }
            else
            {
                //시작 불가
                _isStarted = false;
                return false;
            }
        }

        public void Clear()
        {
            _mapType = Define.MapType.None;
            _gameMode = Define.GameMode.None;
        }

    }

    class LobbyRoom
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
            lock (_lock)
            {
                foreach (Player player in _playerDic.Values)
                {
                    player.Session.Send(packet);
                }
            }
        }

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

                //_players에 내가 포함 되어 있음.
                S_AllPlayerList sPkt1 = new S_AllPlayerList();

                foreach(Player player in PlayerManager.Instance._players.Values)
                {
                    S_AllPlayerList.OnLinePlayer temp = new S_AllPlayerList.OnLinePlayer();
                    temp.CGUID = player.Session.SessionId;
                    temp.Level = player.Info.Level;
                    temp.playerNickName = player.Info.NickName;

                    sPkt1.onLinePlayers.Add(temp);
                }
                session.Send(sPkt1.Write());
            }
        }

        public void HandleLobbytToGameRoom(int CGUID, int roomId)
        {
            lock (_lock)
            {
                bool isPlayerEntered = MoveLobbyToGameRoom(CGUID, roomId);

                S_LobbyToGame sPkt = new S_LobbyToGame();
                sPkt.IsPlayerEntered = isPlayerEntered;
                sPkt.CGUID = CGUID;
                sPkt.roomId = roomId;

                //게임 룸 안에 있는 플레이어에게만 보내 줌(방근 들어간 플레이어 포함)
                if (isPlayerEntered)
                    RoomManager.Instance.GetGameRoom(roomId).Broadcast(sPkt.Write());
                else
                {
                    //못들어갔을때 입장을 시도한 셰션에게만 보내줌
                    ClientSession session = SessionManager.Instance.Find(CGUID);
                    session.Send(sPkt.Write());
                }
            }
        }

        public void HandleGameRoomToLobby(C_GameToLobby cPkt)
        {
            lock (_lock)
            {
                GameRoom gameRoom = GetGameRoom(cPkt.roomId);
                int Owner = MoveGameRoomToLobby(cPkt.CGUID, cPkt.roomId);

                S_GameToLobby sPkt = new S_GameToLobby();
                sPkt.CGUID = cPkt.CGUID;
                sPkt.roomId = cPkt.roomId;

                if (Owner != -1)
                {
                    sPkt.newOwner = Owner;
                    gameRoom.Broadcast(sPkt.Write());
                }

                ClientSession session = SessionManager.Instance.Find(sPkt.CGUID);
                session.Send(sPkt.Write());
            }
        }

        public void HandleCreateGameRoom(C_CreateGameRoom cPkt)
        {
            lock (_lock)
            {
                GameRoom gameRoom = new GameRoom();
                gameRoom.roomOwner = cPkt.CGUID;
                _lobbyRoom.LeaveLobbyRoom(cPkt.CGUID);
                gameRoom.EnterGameRoom(cPkt.CGUID);
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
                    tempRoom.isStarted = gameRoom.Value._isStarted;
                    foreach (Player player in gameRoom.Value._playerList)
                    {
                        S_GetGameRooms.GameRoomlist.PlayerList tempPlayer = new S_GetGameRooms.GameRoomlist.PlayerList();
                        tempPlayer.CGUID = player.Session.SessionId;
                        tempPlayer.isReady = player.Info.isPlayerReady;
                        tempRoom.playerLists.Add(tempPlayer);
                    }

                    sPkt.gameRoomlists.Add(tempRoom);
                }

                session.Send(sPkt.Write());
            }
        }

        public void HandleReadyInGameRoom(C_ClickReadyOnOff cPkt)
        {
            lock (_lock)
            {
                GameRoom room = GetGameRoom(cPkt.roomId);
                if (room == null)
                    Console.WriteLine("there is no such a room");

                Player player = PlayerManager.Instance.GetPlayer(cPkt.CGUID);

                player.Info.isPlayerReady = cPkt.isReady;
                room.SetReady(cPkt.isReady);

                S_ClickReadyOnOff sPkt = new S_ClickReadyOnOff();
                sPkt.roomId = cPkt.roomId;
                sPkt.isReady = cPkt.isReady;
                sPkt.CGUID = cPkt.CGUID;

                room.Broadcast(sPkt.Write());
            }
        }

        public void HandleGameStart(int roomID)
        {
            lock(_lock)
            {
                GameRoom room = GetGameRoom(roomID);
                bool start = room.GameStart();

                S_GameStart sPkt = new S_GameStart();
                sPkt.isStart = start;
                sPkt.roomID = roomID;

                if (start)
                {
                    room.Broadcast(sPkt.Write());
                }
                else
                {
                    ClientSession session = SessionManager.Instance.Find(room.roomOwner);
                    session.Send(sPkt.Write());
                }
            }
        }


        //게임 룸에서 로비로 나옴
        //방에 정상적으로 입장헀다면 true
        //방 안이 꽉찼다면 false 
        public bool MoveLobbyToGameRoom(int CGUID, int roomId)
        {
            lock (_lock)
            {
                GameRoom gameRoom = GetGameRoom(roomId);
                _lobbyRoom.LeaveLobbyRoom(CGUID);

                if (gameRoom.GetPlayerCount() < 4)
                {
                    gameRoom.EnterGameRoom(CGUID);
                    return true;
                }
                else
                    return false;
            }
        }

        //로비 -> 게임 룸 입장   
        //나온 룸에서 내가 마지막이였다면 : true
        //누군가 안에 있다면 false
        public int MoveGameRoomToLobby(int leavePlayerCGUID, int roomId)
        {
            lock (_lock)
            {
                GameRoom gameRoom = GetGameRoom(roomId);

                gameRoom.LeaveGameRoom(leavePlayerCGUID);
                _lobbyRoom.EnterLobbyRoom(leavePlayerCGUID);

                if (gameRoom.GetPlayerCount() == 0)
                {
                    RemoveGameRoom(roomId);
                    return -1;
                }
                else
                {
                    if (leavePlayerCGUID == gameRoom.roomOwner)
                        return gameRoom.SetNewOwner();
                    else
                        return gameRoom.roomOwner;
                }
            }
        }

        public void RemoveGameRoom(int roomId)
        {
            lock (_lock)
            {
                GameRoom room = GetGameRoom(roomId);
                _gameRooms.Remove(roomId);
            }
        }

        public void RoomAllClear()
        {
            _gameRooms.Clear();
        }
    }
}
