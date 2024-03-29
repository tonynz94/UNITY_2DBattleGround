using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.ShowPopupUI<UI_LobbyPopup>();
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_IobbyBGM");

        return true;
    }
}
