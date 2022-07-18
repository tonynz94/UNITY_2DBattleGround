using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameRoom : UI_Popup
{
    //방 ID
    int _roomID;
    int _countPlayer = 0;
    //방 안에 플레이어
    Dictionary<int, Player> playerList = new Dictionary<int, Player>();
    Color _readyColor = new Color(239 / 255f, 88 / 255f, 80 / 255f, 1f);
    Color _cancelColor = new Color(101 / 255f, 158 / 255f, 240 / 255f, 1f);

    enum CharacterItem
    {
        UI_CharacterItem_1,
        UI_CharacterItem_2,
        UI_CharacterItem_3,
        UI_CharacterItem_4
    }

    enum Buttons
    {
        ReadyButton,
        BackButton,
    }

    enum Images
    {
        MapImage
    }

    enum Texts
    {
        TitleText,
        MapImageText,
        ReadyButtonText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Bind<UI_CharacterItem>(typeof(CharacterItem));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindButton(typeof(Buttons));

        BindEvent(GetButton((int)Buttons.ReadyButton).gameObject, OnReadyButton);
        BindEvent(GetButton((int)Buttons.BackButton).gameObject, OnBackButton);

        return true;
    }

    //방 만들사람만 이 함수를 실행하게 됨
    public void SetRoom(int roomID, Player ownerPlayer)
    {
        _roomID = roomID;
        Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_1).PlayerEnter(isOwner: true);
        Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_2).PlayerLeave();
        Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_3).PlayerLeave();
        Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_4).PlayerLeave();

        playerList.Add(ownerPlayer._CGUID, ownerPlayer);
        GameRoom gameRoom = Managers.Room.GetGameRoom(_roomID);
        gameRoom.AddPlayer(ownerPlayer._CGUID);

        RefreashTitle();
        RefreashMap();
    }

    public void OnReadyButton()
    {
        bool ready = Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_1)._ready;
        if (ready)  //true
        {
            GetText((int)Texts.ReadyButtonText).text = "READY";
            GetButton((int)Buttons.ReadyButton).GetComponent<Image>().color = _readyColor;
            Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_1).ReadyOff();
        }
        else
        {
            GetText((int)Texts.ReadyButtonText).text = "CANCEL";
            GetButton((int)Buttons.ReadyButton).GetComponent<Image>().color = _cancelColor; ;
            Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_1).ReadyOn();
        }
        Debug.Log("Ready Clicked");
    }

    public void OnBackButton()
    {
        Managers.UI.ClosePopupUI();
        Debug.Log("Back Clicked");
    }

    public void EnterMatchRoom(int CGUID)
    {
        Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_2).PlayerEnter();
        Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_3).PlayerEnter();
        Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_4).PlayerEnter();

        playerList.Add(CGUID, Managers.Player.GetPlayer(CGUID));
        GameRoom gameRoom = Managers.Room.GetGameRoom(_roomID);
        gameRoom.AddPlayer(CGUID);

        RefreashTitle();
    }

    public void RefreashTitle()
    {
        GetText((int)Texts.TitleText).text = $"Match Lobby ({playerList.Count}/4)";
    }

    public void RefreashMap()
    {
        switch (Managers.Room.GetGameRoom(_roomID)._mapType)
        {
            case Define.MapType.GrassMap:
                GetImage((int)Images.MapImage).sprite = Managers.Map.GetMapSprite(Define.MapType.GrassMap);
                GetText((int)Texts.MapImageText).text = "Grass Map";
                break;
            case Define.MapType.LakeMap:
                GetImage((int)Images.MapImage).sprite = Managers.Map.GetMapSprite(Define.MapType.LakeMap);
                GetText((int)Texts.MapImageText).text = "Lake Map";
                break;
            case Define.MapType.TreeMap:
                GetImage((int)Images.MapImage).sprite = Managers.Map.GetMapSprite(Define.MapType.TreeMap);
                GetText((int)Texts.MapImageText).text = "Tree Map";
                break;
        }
    }

    public void LeaveMatchRoom()
    {

    }

    public void SetMyCharacterSlot()
    {
        
    }

    public void SetReady()
    {

    }
}
