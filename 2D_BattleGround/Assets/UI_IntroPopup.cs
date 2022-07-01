using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_IntroPopup : UI_Popup
{
    protected enum Buttons
    {
        StartButton,
        ExitButton
    }

    // Start is called before the first frame update
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.StartButton).gameObject.BindEvent(OnStartButton);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(OnExitButton);
        
        return true;
    }

    void OnStartButton()
    {
        Debug.Log("게임 시작");
    }

    void OnExitButton()
    {
        Debug.Log("게임 끝내기");
    }
}
