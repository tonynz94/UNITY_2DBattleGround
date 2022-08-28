using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum GameState
    {
        None,
        Waiting,
        Started,
    }
}

public enum MESSAGE_EVENT_TYPE
{
    MESS_CHATTING_ADD,
    MESS_ALLNOTICE_ADD,
    MESS_ROOMLIST_SELECT,
    MESS_PLAYERDIE,
    MESS_PLAYERWINNER,
    MESS_PLAYERDEATH,
    MESS_MAXCOUNT

}


public class MessageSystem
{
    static Action<object>[] _eventProcDelegates = new Action<object>[(int)MESSAGE_EVENT_TYPE.MESS_MAXCOUNT];

    public static void CallEventMessage(MESSAGE_EVENT_TYPE evtType, object obj = null)
    {
        _eventProcDelegates[(int)evtType]?.Invoke(obj);
    }

    public static void RegisterMessageSystem(int evtType, Action<object> evt)
    {
        _eventProcDelegates[evtType] += evt;
    }

    public static void UnRegisterMessageSystem(int evtType, Action<object> evt)
    {
        _eventProcDelegates[evtType] -= evt;
    }
}

public class KillDeath
{
    public int _attackerCGUID;
    public int _deathCGUID;
}
