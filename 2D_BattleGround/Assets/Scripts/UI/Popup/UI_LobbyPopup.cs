using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static Define;

public class UI_LobbyPopup : UI_Popup
{
    enum Buttons
    {
        ShowRoomButton,
        CreateRoomButton,
        HeroButton,
    }

    enum Texts
    {
        ShowRoomButtonText,
        CreateRoomButtonText,
        HeroButtonText,
        PlayerNickText,
        PlayerLevelText,
        MoneyText,
        DiamondText,
        ChatText,

    }

    enum Images
    {
        ProfileImage,
        PlayerAvatorImage,
    }

    enum GameObjects
    {
        PlayerEXPBar
    }

    public void OnEnable()
    {
        MessageSystem.RegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_CHATTING_ADD, OnChatAdd);
        MessageSystem.RegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_CHATTING_ADD, OnAllNoticeAdd);
    }

    public void OnDisable()
    {
        MessageSystem.UnRegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_CHATTING_ADD, OnChatAdd);
        MessageSystem.UnRegisterMessageSystem((int)MESSAGE_EVENT_TYPE.MESS_CHATTING_ADD, OnAllNoticeAdd);
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        BindEvent(GetButton((int)Buttons.ShowRoomButton).gameObject, OnShowRoomButton);
        BindEvent(GetButton((int)Buttons.CreateRoomButton).gameObject, OnCreateButton);

        BindEvent(GetButton((int)Buttons.HeroButton).gameObject, OnHeroButton);

        InitPlayerInfo();

        return true;
    }

    public void OnShowRoomButton()
    {
        Debug.Log("Show Room");
        Managers.UI.ShowPopupUI<UI_RoomList>();
    }

    public void OnCreateButton()
    {
        Debug.Log("Create Room");
    }

    public void OnHeroButton()
    {
        Debug.Log("My Hero List");
    }

    public void InitPlayerInfo()
    {
        GetText((int)Texts.PlayerNickText).text = Managers.Player.MyPlayer.NickName;
        GetText((int)Texts.PlayerLevelText).text = Managers.Player.MyPlayer.Level.ToString();
        GetText((int)Texts.DiamondText).text = Managers.Player.MyPlayer._gameDiamond.ToString();
        GetText((int)Texts.MoneyText).text = Managers.Player.MyPlayer._gameMoney.ToString();
    }

    public void OnChatAdd(object obj)
    {      
        
    }

    public void OnAllNoticeAdd(object obj)
    {

    }
}
