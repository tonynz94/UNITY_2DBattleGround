using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    public bool _byJoyStick;
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.IntroScene;
        Managers.UI.ShowPopupUI<UI_IntroPopup>();
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_IntroBGM");
        Define.ByJoyStick = _byJoyStick;
        return true;
    }
}
