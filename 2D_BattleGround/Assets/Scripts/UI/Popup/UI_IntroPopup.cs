using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_IntroPopup : UI_Popup
{
    protected enum Buttons
    {
        TapToStartButton,
    }

    // Start is called before the first frame update
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.TapToStartButton).gameObject.BindEvent(OnTapToStartButton);
        
        return true;
    }

    void OnTapToStartButton()
    {
        Debug.Log("start Button");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_MainButton");
        Managers.Net.ConnectServer();
    }

}
