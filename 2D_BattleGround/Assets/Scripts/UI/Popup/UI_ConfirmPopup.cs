using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class UI_ConfirmPopup : UI_Popup
{
    enum Buttons
    {
        StartButton,
    }

    enum GameObjects
    {
        IDInputField,
    }

    enum Texts
    {
        StartButtonText,
    }

    TMP_InputField inputField;

    // Start is called before the first frame update
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindObject(typeof(GameObjects));

        BindEvent(GetButton((int)Buttons.StartButton).gameObject, OnStartButtonClick);

        inputField = GetObject((int)GameObjects.IDInputField).gameObject.GetComponent<TMP_InputField>();
        inputField.text = "";
        return true;
    }

    public void OnStartButtonClick(PointerEventData evt)
    {
        Debug.Log("StartButton Click");
        if (string.IsNullOrEmpty(inputField.text) || inputField.text.Length > 11)
        {
            Debug.Log("아이디를 입력하시오");
            return;
        }

        Managers.Player.MyPlayer.NickName = inputField.text;

        C_FirstEnter sPkt = new C_FirstEnter();
        sPkt.playerNickName = inputField.text;
        Debug.Log("[NetworkManager] SEND : C_FirstEnter");
        Managers.Net.Send(sPkt.Write());

    }
}
