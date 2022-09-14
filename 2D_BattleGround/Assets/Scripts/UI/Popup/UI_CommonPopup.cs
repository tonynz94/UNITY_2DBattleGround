using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CommonPopup : UI_Popup
{
    public Action _onClickYesButton = null;

    enum Buttons
    {
        YesButton,
        NoButton,
    }

    enum Texts
    {
        TitleText,
        ContentText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.YesButton).gameObject, OnYesButton);
        BindEvent(GetButton((int)Buttons.NoButton).gameObject, OnNoButton);

        return true;
    }

    public void SetPopupCommon(Define.PopupCommonType popupType, string titleText, string contentText, Action OnClickYesButton = null)
    {
        GetText((int)Texts.TitleText).text = titleText;
        GetText((int)Texts.ContentText).text = contentText;
        _onClickYesButton = OnClickYesButton;
        switch (popupType)
        {
            case Define.PopupCommonType.YES:
                GetButton((int)Buttons.YesButton).gameObject.SetActive(true);
                GetButton((int)Buttons.YesButton).gameObject.SetActive(false);
                break;
            case Define.PopupCommonType.YESNO:
                GetButton((int)Buttons.YesButton).gameObject.SetActive(true);
                GetButton((int)Buttons.YesButton).gameObject.SetActive(true);
                break;
        }
    }

    public void OnYesButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
        //Managers.Sound.Play(Define.Sound.Effect, "Sound_CheckButton");
        if (_onClickYesButton != null)
            _onClickYesButton.Invoke();
    }

    public void OnNoButton(PointerEventData evt)
    {
        //Managers.Sound.Play(Define.Sound.Effect, "Sound_CheckButton");
        Managers.UI.ClosePopupUI();
    }
}
