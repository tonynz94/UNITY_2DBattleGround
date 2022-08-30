using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class PlayerInfo
    {

        public String NickName;

        public int Level = 1;
        public int money = 10000;
        public int diamond = 300;
        public int _level = 1;
        public int _totalExp = 200;
        public int _currentExp = 0;
        public int SpeedUpPoint = 0;
        public int RangeUpPoint = 0;
        public int PowerUpPoint = 0;
        public int WaterCountUpPoint = 0;

        public int cellPosX;
        public int cellPosY;
        public Define.ObjectState State;
        public Define.MoveDir Dir;

        public bool isInGameRoom = false;
        public bool isGameOwner = false;
        public bool isPlayerReady = false;
    }

    class Player
    {
        public PlayerInfo Info { get; set; } = new PlayerInfo(); 
        public ClientSession Session { get; set; }
        public GameRoom Room { get; set; }

        public void LeaveFromGameRoom()
        {
            Info.isInGameRoom = false;
            Info.isGameOwner = false;
            Info.isPlayerReady = false;
        }
    }
}
