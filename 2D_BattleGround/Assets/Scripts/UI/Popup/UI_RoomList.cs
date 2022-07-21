using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RoomList : UI_Popup
{
    enum Objects
    {
        NoRoomObject,
        RoomExistObject,
        RoomItemContentObject
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

        BindEvent(GetButton((int)Buttons.JoinButton).gameObject, OnJoinButton);
        BindEvent(GetButton((int)Buttons.BackButton).gameObject, OnBackButton);

        return true;
    }

    public void RefreshGameRoom()
    {
        GameObject parent = GetObject((int)Objects.RoomItemContentObject);
        //Managers.Resource.Instantiate("UI/SubItem/UI_ChatItem", GetObject((int)GameObjects.ChatDashBoardObject).transform);

        foreach(GameRoom gameRoom in Managers.Room._gameRooms.Values)
        {
            UI_GameRoomItem item = Managers.Resource.Instantiate("UI/SubItem/UI_GameRoomItem", parent.transform).GetComponent<UI_GameRoomItem>();
            item.SetRoomItemNotice(gameRoom.roomId, gameRoom.roomOwner, gameRoom._mapType, gameRoom.GetPlayerCount(), Define.GameState.Waiting);
        }
    }

    public void OnJoinButton()
    {
        Debug.Log("Join Button Clicked");
    }

    public void OnBackButton()
    {
        Managers.Room.GameRoomAllClear();
        Managers.UI.ClosePopupUI(this);
        Debug.Log("Close Button Click");
    }
}
