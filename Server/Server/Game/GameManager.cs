﻿using System;
using System.Collections.Generic;
using System.Text;
using static Server.Define;

namespace Server.Game
{
    class GameField
    {
        object _lock = new object();

        int _roomID;
        MapType _mapID;
        GameMode _gameMode;
        Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        public GameField(int roomID, MapType mapID, GameMode gameMode)
        {
            _roomID = roomID;
            _mapID = mapID;
            _gameMode = gameMode;
        }

        public void EnterToField(int CGUID)
        {
            lock (_lock)
            {
                Player player = PlayerManager.Instance.GetPlayer(CGUID);
                if (player == null)
                {
                    Console.WriteLine("Theres no player in Game plz check sink");
                    return;
                }

                GameRoom gameRoom = RoomManager.Instance.GetGameRoom(_roomID);

                if (gameRoom == null && gameRoom._isStarted == false)
                {
                    Console.WriteLine("Theres no GameRoom or Its not Started yet");
                    return;
                }

                _playerDic.Add(CGUID, PlayerManager.Instance.GetPlayer(CGUID));

                Random rand = new Random();
                //본인에게 전송
                {
                    foreach (var others in gameRoom._playerList)
                    {
                        //ClientSession session = SessionManager.Instance.Find(CGUID);
                        S_EnterFieldWorld sPkt = new S_EnterFieldWorld();
                        sPkt.CGUID = CGUID;
                        sPkt.posX = rand.Next(-5, 5);
                        sPkt.posY = 0;
                        sPkt.posZ = rand.Next(-5, 5);
                        others.Session.Send(sPkt.Write());
                    }
                }
            }
        }

        public void PlayerMove(C_Move cPkt)
        {
            // 좌표 바꿔주고
            Player player = PlayerManager.Instance.GetPlayer(cPkt.CGUID);
            player.Info.cellPosX = cPkt.cellPosX;
            player.Info.cellPosY = cPkt.cellPosY;
            player.Info.Dir = (Define.MoveDir)cPkt.Dir;
            player.Info.State = (Define.ObjectState)cPkt.State;

            // 모두에게 알린다
            S_BroadcastMove sPkt = new S_BroadcastMove();
            sPkt.playerId = player.Session.SessionId;
            sPkt.posX = cPkt.posX;
            sPkt.posY = cPkt.posY;
            sPkt.cellPosX = (int)player.Info.cellPosX;
            sPkt.cellPosY = (int)player.Info.cellPosY;
            sPkt.Dir = (int)player.Info.Dir;
            sPkt.State = (int)player.Info.State;
            Broadcast(sPkt.Write());
        }

        public void Broadcast(ArraySegment<byte> packet)
       {
           lock (_lock)
           {
               foreach (Player player in _playerDic.Values)
               {
                   player.Session.Send(packet);
               }
           }
       }
    }
    //실행되고 있는 게임들
    class GameManager
    {
        public static GameManager Instance { get; } = new GameManager();

        public Dictionary<int, GameField> _playingGame = new Dictionary<int, GameField>();

        public void NewGameStart(int roomID)
        {
            MapType mapType = RoomManager.Instance.GetGameRoom(roomID)._mapType;
            GameMode gameMode = RoomManager.Instance.GetGameRoom(roomID)._gameMode;
            _playingGame.Add(roomID, new GameField(roomID ,mapType, gameMode));
        }
        
        public GameField GetGameField(int roomID)
        {
            GameField gameField = null;
            _playingGame.TryGetValue(roomID, out gameField);

            return gameField;
        }

        public void HandlePlayerEnterToField(int roomID,int CGUID)
        {
            GameField gameField = null;
            _playingGame.TryGetValue(roomID, out gameField);

            if (gameField == null)
            {
                Console.WriteLine("Theres is no GameID Started plz Check");
            }
            else
            {
                gameField.EnterToField(CGUID);
            }
        }

        public void HandleMove(C_Move cPkt)
        {
            GameField gameField = GetGameField(cPkt.roomID);

            if (gameField == null)
            {
                Console.WriteLine("Not Exist GameRoom or on Field");
                return;
            }

            gameField.PlayerMove(cPkt);
        }
    }
}
