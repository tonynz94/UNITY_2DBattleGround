using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IntroPopup : UI_Popup
{
    protected enum Buttons
    {
        StartButton,
        ContinueButton
    }

    // Start is called before the first frame update
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnStartButton);
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnContinueButton);
        
        return true;
    }

    //????
    void OnStartButton()
    {
        Debug.Log("start Button");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_MainButton");
        Managers.UI.ClosePopupUI(this);
        Managers.UI.ShowPopupUI<UI_ConfirmPopup>();
    }

    //???
    void OnContinueButton()
    {
        Debug.Log("ContinueButton");
        Managers.Sound.Play(Define.Sound.Effect, "Sound_MainButton");

        //ToDo
        //????? ??? ??
        //??? ??? ?? ? ??? ??? ??
    }
}
