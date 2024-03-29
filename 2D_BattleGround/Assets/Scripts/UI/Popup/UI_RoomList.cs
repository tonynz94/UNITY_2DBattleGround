using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_RoomList : UI_Popup
{
    Dictionary<int, UI_GameRoomItem> _gameRoomItem = new Dictionary<int, UI_GameRoomItem>();
    int _selectRoomId = 0;
    enum Objects
    {
        NoRoomObject,
        RoomExistObject,
        RoomItemContentObject,

        Slot1OnObject,
        Slot2OnObject,
        Slot3OnObject,
        Slot4OnObject,

        Slot1OffObject,
        Slot2OffObject,
        Slot3OffObject,
        Slot4OffObject,
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

        MapImageText,
        GameTypeText,

        Slot1NickName,
        Slot2NickName,
        Slot3NickName,
        Slot4NickName,
    }

    enum Images
    {
        MapImage,
        GameTypeImage,

        Slot1CharImage,
        Slot2CharImage,
        Slot3CharImage,
        Slot4CharImage,
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

        foreach(GameRoom gameRoom in Managers.Room._gameRooms.Values)
        {
            UI_GameRoomItem item = Managers.Resource.Instantiate("UI/SubItem/UI_GameRoomItem", parent.transform).GetComponent<UI_GameRoomItem>();
            item.SetRoomItemNotice(gameRoom.roomId, gameRoom.roomOwner, gameRoom._mapType, gameRoom.GetPlayerCount(), gameRoom.isStarted ? Define.GameState.Started : Define.GameState.Waiting);
            _gameRoomItem.Add(gameRoom.roomId, item);
        }
    }

    public void OnJoinButton(PointerEventData evt)
    {
        Debug.Log("Join Button Clicked");
        if(_selectRoomId == -1)
        {
            Debug.Log("There is no room select ");
            return;
        }

        if(Managers.Room.GetGameRoom(_selectRoomId).isStarted)
        {
            Managers.UI.ShowPopupUI<UI_CommonPopup>().SetPopupCommon(
                Define.PopupCommonType.YES, "Game Started", "Already Game Started join waiting room" ,
                () => { Managers.UI.ClosePopupUI(this); } );

            Debug.Log("is already Started");
            return;
        }

        if(Managers.Room.GetGameRoom(_selectRoomId).GetPlayerCount() == 4)
        {
            Managers.UI.ShowPopupUI<UI_CommonPopup>().SetPopupCommon(
            Define.PopupCommonType.YES, "Full", "Its already Full Room",
            () => { Managers.UI.ClosePopupUI(this); });

            Debug.Log("No slot to go in");
            return;
        }

        C_LobbyToGame sPkt = new C_LobbyToGame();

        sPkt.CGUID = Managers.Player.GetMyCGUID();
        sPkt.roomId = _selectRoomId;

        Managers.Net.Send(sPkt.Write());
    }

    public void OnBackButton(PointerEventData evt)
    {
        Managers.Room.GameRoomAllClear();
        Managers.UI.ClosePopupUI(this);
        Debug.Log("Close Button Click");
    }

    public void OnRoomListItemClicked(object obj)
    {
        _selectRoomId = (int)obj;
        GameRoom room = Managers.Room.GetGameRoom(_selectRoomId);

        GetObject((int)Objects.NoRoomObject).SetActive(false);
        GetObject((int)Objects.RoomExistObject).SetActive(true);

        GetImage((int)Images.MapImage).sprite = Managers.Map.GetMapSprite(room._mapType);
        GetText((int)Texts.MapImageText).text = System.Enum.GetName(typeof(Define.MapType), (int)room._mapType);
        GetImage((int)Images.GameTypeImage).sprite = Managers.Map.GetGameType(room._gameMode);
        GetText((int)Texts.GameTypeText).text = System.Enum.GetName(typeof(Define.GameMode), (int)room._gameMode);
        GetButton((int)Buttons.JoinButton).interactable = !room.isStarted;

        int i = 1;
        foreach(var player in room._playerList)
        {
            GetObject((int)System.Enum.Parse(typeof(Objects), $"Slot{i}OffObject")).SetActive(false);
            GetObject((int)System.Enum.Parse(typeof(Objects), $"Slot{i}OnObject")).SetActive(true);
            GetText((int)System.Enum.Parse(typeof(Texts), $"Slot{i}NickName")).text = player.NickName;
            i++;
        }

        foreach (UI_GameRoomItem item in _gameRoomItem.Values)
        {
            item.UnSelected();
        }
    }
}
