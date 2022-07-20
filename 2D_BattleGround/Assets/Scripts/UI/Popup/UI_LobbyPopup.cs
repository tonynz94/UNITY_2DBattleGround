using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Define;

public class UI_LobbyPopup : UI_Popup
{
    enum Buttons
    {
        ShowRoomButton,
        CreateRoomButton,
        HeroButton,
        SendChatButton,
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
        PlayerEXPBar,
        ChatDashBoardObject,
        ChatContentField
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
        BindEvent(GetButton((int)Buttons.SendChatButton).gameObject, OnSendChatButton);

        InitPlayerInfo();

        //채팅 자주 사용할꺼같아서 먼저 로드하면 최적화에 좋을꺼같지만 나중에...
        //Managers.Resource.Load<GameObject>("Prefabs/SubItem/UI_chatItem");

        return true;
    }

    public void OnShowRoomButton()
    {
        Debug.Log("Show Room");
        C_GetGameRooms cPkt = new C_GetGameRooms();
        Managers.Net.Send(cPkt.Write());

        
    }

    public void OnCreateButton()
    {
        Managers.UI.ShowPopupUI<UI_CreateRoom>();
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

    public void OnSendChatButton()
    {
        string chatStr = GetObject((int)GameObjects.ChatContentField).GetComponent<TMP_InputField>().text;

        if (string.IsNullOrEmpty(chatStr))
            return;

        C_SendChat sPkt = new C_SendChat();
        sPkt.messageType = (int)Define.ChatType.Channel;
        sPkt.nickName = Managers.Player.MyPlayer.NickName;
        sPkt.chatContent = chatStr;

        Managers.Net.Send(sPkt.Write());

        GetObject((int)GameObjects.ChatContentField).GetComponent<TMP_InputField>().text = "";
    }

    public void OnChatAdd(object obj)
    {
        ChatPiece chatPiece = obj as ChatPiece;
        GameObject go =Managers.Resource.Instantiate("UI/SubItem/UI_ChatItem", GetObject((int)GameObjects.ChatDashBoardObject).transform);
        go.GetComponent<UI_ChatItem>().SetChatPiece(chatPiece._chatType, chatPiece._nickName, chatPiece._chatContent);
    }

    public void OnAllNoticeAdd(object obj)
    {

    }
}
