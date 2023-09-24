using Server.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class PlayerInfo
    {
        public String NickName;

        public int Level = 1;
        public int maxHP = 200;
        public int currentHP = 200;
        public int money = 10000;
        public int diamond = 300;
        public int _level = 1;
        public int _totalExp = 200;
        public int _damage = 100;
        public int _currentExp = 0;
        public int SpeedUpPoint = 0;
        public int RangeUpPoint = 0;
        public int PowerUpPoint = 0;
        public int WaterCountUpPoint = 0;
        public int currentWaterBoomCountInField = 0;

        public int cellPosX;
        public int cellPosY;
        public Define.ObjectState State;
        public Define.MoveDir Dir;

        public bool isInGameRoom = false;
        public bool isGameOwner = false;
        public bool isPlayerReady = false;

        public EventHandler<UserLoginEvent> userLoginEventHandler { get; } = new ();

        public int Exp
        {
            get
            {
                return _currentExp;
            }
            set
            {
                _currentExp = value;

                int level = _level;
                while(true)
                {
                    LevelStat stat;
                    if (DataManager.LevelStatDict.TryGetValue(_level, out stat) == false)
                        break;
                    if (_currentExp < stat.totalEXP)
                        break;

                    Console.WriteLine("1Level UP");
                    level++;
                }

                if(level != _level)
                {
                    //TODO
                    //패킷 보내주면 됌.
                }
            }
        }
    }

    class Player
    {
        public PlayerInfo Info { get; set; } = new PlayerInfo(); 
        public GameRoom Room { get; set; }
        public long SessionId { get; init; }

        public bool isPossableWaterBoomSettedInField()
        {
            int maxWaterBoom= Data.DataManager.WaterCountUpDict[Info.WaterCountUpPoint].waterMaxCount;

            if (Info.currentWaterBoomCountInField < maxWaterBoom)
                return true;
 
            return false;
        }

        public void SetWaterBoomCountUp()
        {
            // increment
            Info.currentWaterBoomCountInField++;
        }

        public void SetWaterBoomCountDown()
        {
            Info.currentWaterBoomCountInField--;
        }

        public void LeaveFromGameRoomOrField()
        {
            Info.isInGameRoom = false;
            Info.isGameOwner = false;
            Info.isPlayerReady = false;
            Info.maxHP = 200;
            Info.currentHP = 200;
            Info.currentWaterBoomCountInField = 0;
        }

        private void Reset() {
            
        }
    }
}
