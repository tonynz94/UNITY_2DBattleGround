using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_GameRoomItem : UI_Base
{
    bool _selected = false;
    int _roomId;
    Color _selectedColor = Color.cyan;
    Color _unSelectedColor = Color.gray;

    GameState state = GameState.None;


    enum Images
    {
        BGImage
    }

    enum Buttons
    {
        GameRoomButton,
    }

    enum Texts
    {
        OwnerText,
        MapText,
        PlayersText,
        StateText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        BindEvent(GetButton((int)Buttons.GameRoomButton).gameObject, OnGameRoomButton);

        _selected = false;
        return true;
    }

    public void SetRoomItemNotice(int roomId, int Owner, MapType mapType, int playersCount, GameState state)
    {
        _roomId = roomId;
        GetText((int)Texts.OwnerText).text = Managers.Player.GetPlayerNick(Owner);
        GetText((int)Texts.MapText).text = System.Enum.GetName(typeof(MapType), (int)mapType);
        GetText((int)Texts.PlayersText).text = $"({playersCount}/4)";
        GetText((int)Texts.StateText).text = System.Enum.GetName(typeof(GameState), (int)state);
    }

    public void OnGameRoomButton(PointerEventData evt)
    {
        Debug.Log($"{_roomId}Item Clicked");
        MessageSystem.CallEventMessage(MESSAGE_EVENT_TYPE.MESS_ROOMLIST_SELECT, _roomId);
        Selected();
    }

    public void Selected()
    {
        _selected = true;
        GetImage((int)Images.BGImage).color = _selectedColor;
    }

    public void UnSelected()
    {
        _selected = false;
        GetImage((int)Images.BGImage).color = _unSelectedColor;
    }
}
