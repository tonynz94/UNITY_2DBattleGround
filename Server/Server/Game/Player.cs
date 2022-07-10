using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class PlayerInfo
    {
        public bool isInGame;
        public float PosX;
        public float PosY;
        public Define.ObjectState State;
        public Define.MoveDir Dir;
    }

    class Player
    {
        public PlayerInfo Info { get; set; }
        public ClientSession Session { get; set; }
        public GameRoom Room { get; set; }
    }
}
