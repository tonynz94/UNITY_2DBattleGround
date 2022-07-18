using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{

    public class Define
    {
        public enum MapType
        {
            None,
            TreeMap,
            LakeMap,
            GrassMap,
        }

        public enum GameMode
        {
            None,
            PvPMode,
            CooperativeMode,
        }


        public enum ChatType
        {
            System,
            AllNotices,
            Channel,

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

        public enum WorldObject
        {
            Unknown,
            Player,
            Monster,
            Boss
        }


        public enum Scene
        {
            Unknown,
            IntroScene,
            LobbyScene,
            LoadingScene,
            GameScene,
        }

        public enum CameraMode
        {
            AliveMode,
            DieMode,
            GameOverMode,
            EndingMode
        }
        public enum UIEvent
        {
            Click,
            Pressed,
            PointerDown,
            PointerUp,
        }

        public enum Sound
        {
            Bgm,
            Effect,
            Speech,
            Max
        }
    }
}
