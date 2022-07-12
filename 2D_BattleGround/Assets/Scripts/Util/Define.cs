using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum ChatType
    {
        System,
        Channel
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

    public enum GameMode
    {
        PvPMode,
        CooperativeMode,
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



    public enum MESSAGE_EVENT_TYPE
    {
        MESS_MAXCOUNT
    }


    public class MessageSystem
    {
        static Action<object>[] _eventProcDelegates = new Action<object>[(int)MESSAGE_EVENT_TYPE.MESS_MAXCOUNT];

        public static void CallEventMessage(int evtType, object obj)
        {
                _eventProcDelegates[evtType]?.Invoke(obj);
        }

        public static void RegisterMessageSystem(int evtType, Action<object> evt)
        {
            _eventProcDelegates[evtType] += evt;
        }

        public static void UnRegiserMessageSystem(int evtType, Action<object> evt)
        {
            _eventProcDelegates[evtType] -= evt;
        }

        

    }

}
