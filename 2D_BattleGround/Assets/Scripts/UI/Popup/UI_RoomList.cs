using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RoomList : UI_Popup
{
    enum Objects
    {
        NoRoomObject,
        RoomExistObject
    }

    enum Buttons
    {
        JoinButton,
        BackButton
    }

    enum Texts
    {
        JoinButtonText,
        BGTitleText,
    }

    enum Images
    {
        MapImage
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));

        BindEvent(GetObject((int)Buttons.JoinButton), OnJoinButton);
        BindEvent(GetObject((int)Buttons.BackButton), OnBackButton);

        //TODO
        //서버로부터 룸 정보를 받아


        return true;
    }

    public void OnJoinButton()
    {
        Debug.Log("Join Button Clicked");
    }

    public void OnBackButton()
    {
        Managers.UI.ClosePopupUI(this);
        Debug.Log("Close Button Click");
    }
}
