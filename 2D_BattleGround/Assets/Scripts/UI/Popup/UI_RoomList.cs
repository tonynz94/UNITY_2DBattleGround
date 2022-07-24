using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RoomList : UI_Popup
{
    Dictionary<int, UI_GameRoomItem> _gameRoomItem = new Dictionary<int, UI_GameRoomItem>();
    int _selectRoomId = 0;
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

        GetObject((int)Objects.RoomExistObject).SetActive(false);

        return true;
    }

    private void OnEnable()
    {
        MessageSystem.RegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_ROOMLIST_SELECT, OnRoomListItemClicked);
    }

    private void OnDisable()
    {
        MessageSystem.UnRegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_ROOMLIST_SELECT, OnRoomListItemClicked);
    }

    public void RefreshGameRoom()
    {
        GameObject parent = GetObject((int)Objects.RoomItemContentObject);
        //Managers.Resource.Instantiate("UI/SubItem/UI_ChatItem", GetObject((int)GameObjects.ChatDashBoardObject).transform);

        foreach(GameRoom gameRoom in Managers.Room._gameRooms.Values)
        {
            UI_GameRoomItem item = Managers.Resource.Instantiate("UI/SubItem/UI_GameRoomItem", parent.transform).GetComponent<UI_GameRoomItem>();
            item.SetRoomItemNotice(gameRoom.roomId, gameRoom.roomOwner, gameRoom._mapType, gameRoom.GetPlayerCount(), Define.GameState.Waiting);
            _gameRoomItem.Add(gameRoom.roomId, item);
        }
    }

    public void OnJoinButton()
    {
        Debug.Log("Join Button Clicked");
        if(_selectRoomId == -1)
        {
            Debug.Log("There is no room select ");
        }
    }

    public void OnBackButton()
    {
        Managers.Room.GameRoomAllClear();
        Managers.UI.ClosePopupUI(this);
        Debug.Log("Close Button Click");
    }

    public void OnRoomListItemClicked(object obj)
    {
        _selectRoomId = (int)obj;
        GetObject((int)Objects.NoRoomObject).SetActive(false);
        GetObject((int)Objects.RoomExistObject).SetActive(true);

        foreach (UI_GameRoomItem item in _gameRoomItem.Values)
        {
            item.UnSelected();
        }
    }
}
