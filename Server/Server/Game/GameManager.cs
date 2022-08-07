using System;
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
            lock(_lock)
            {
                Player player = PlayerManager.Instance.GetPlayer(CGUID);
                if (player == null)
                {
                    Console.WriteLine("Theres no player in Game plz check sink");
                    return;
                }

                GameRoom gameRoom = RoomManager.Instance.GetGameRoom(_roomID);

                if(gameRoom == null && gameRoom._isStarted == false)
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
                        sPkt.posX = rand.Next(-5,5);
                        sPkt.posY = 0;
                        sPkt.posZ = rand.Next(-5,5);
                        others.Session.Send(sPkt.Write());
                    }
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
    }
}
