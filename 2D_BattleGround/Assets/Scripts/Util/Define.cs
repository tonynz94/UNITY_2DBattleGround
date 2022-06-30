using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Scene
    {
        Unknown,
        IntroScene,
        LobbyScene,
        LoadingScene,
        GameScene,
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
