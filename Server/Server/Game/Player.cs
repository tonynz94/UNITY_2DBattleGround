﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class PlayerInfo
    {

        public String NickName;

        public int Level;
        public float cellPosX;
        public float cellPosY;
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
