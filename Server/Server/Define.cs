using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{

    public class Define
    {
        public enum RoomID
        {
            Lobby = 0,
        
        }

        public enum ObjectState
        {
            Idle,
            Moving,
            Attack,
            Dead,
        }

        public enum MoveDir
        {
            None,
            Up,
            Down,
            Left,
            Right,
        }
    }
}
