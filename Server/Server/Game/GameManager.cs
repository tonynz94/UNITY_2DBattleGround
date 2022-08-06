using System;
using System.Collections.Generic;
using System.Text;
using static Server.Define;

namespace Server.Game
{
    class GameField
    {
        object _lock = new object();

        MapType _mapID;
        GameMode _gameMode;
        Dictionary<int, Player> _playerDic = new Dictionary<int, Player>();

        public GameField(MapType mapID, GameMode gameMode)
        {
            _mapID = mapID;
            _gameMode = gameMode;
        }

        public void EnterToField(int CGUID)
        {          
            lock(_lock)
            {
                _playerDic.Add(CGUID, PlayerManager.Instance.GetPlayer(CGUID));

                //본인에게 전송
                {

                }

                //타인에게 전송
                {

                }
            }
        }
    }
    //실행되고 있는 게임들
    class GameManager
    {
        public static GameManager Instance { get; } = new GameManager();

        public Dictionary<int, GameField> _playerGame = new Dictionary<int, GameField>();


    }
}
