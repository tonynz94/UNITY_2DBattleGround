using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class PlayerInfo
    {
        public String NickName;
        public bool isInGame;
        public int Level;
        public float PosX;
        public float PosY;
        public Define.ObjectState State;
        public Define.MoveDir Dir;
    }

    class Player
    {
        public PlayerInfo Info { get; set; } = new PlayerInfo(); 
        public ClientSession Session { get; set; }
        public GameRoom Room { get; set; }
    }
}
