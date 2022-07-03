using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.IntroScene;
        Managers.UI.ShowPopupUI<UI_IntroPopup>();
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_MainPlayBGM");
        return true;
    }
}
