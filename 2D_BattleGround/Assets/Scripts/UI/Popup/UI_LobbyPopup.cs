using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static Define;
using Data;
using UnityEngine.EventSystems;

public class UI_LobbyPopup : UI_Popup
{
    enum Buttons
    {
        ShowRoomButton,
        CreateRoomButton,
        HeroButton,
        SendChatButton,
        SkillButton,
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
        BindEvent(GetButton((int)Buttons.SkillButton).gameObject, OnSkillButton);

        InitPlayerInfo();

        //???? ???? ?????????????? ???? ???????? ???????? ???????????? ??????...
        //Managers.Resource.Load<GameObject>("Prefabs/SubItem/UI_chatItem");

        return true;
    }

    public void OnShowRoomButton(PointerEventData evt)
    {
        Debug.Log("Show Room");
        Managers.Room.GameRoomAllClear();

        C_GetGameRooms cPkt = new C_GetGameRooms();
        Debug.Log("[NetworkManager] SEND : C_GetGameRooms");
        Managers.Net.Send(cPkt.Write());

        
    }

    public void OnCreateButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_CreateRoom>();
    }

    public void OnHeroButton(PointerEventData evt)
    {
        Debug.Log("My Hero List");
    }

    public void InitPlayerInfo()
    {
        int level = Managers.Player.MyPlayer.Level;

        GetText((int)Texts.PlayerNickText).text = Managers.Player.MyPlayer.NickName;
        GetText((int)Texts.PlayerLevelText).text = level.ToString();
        GetText((int)Texts.DiamondText).text = Managers.Player.MyPlayer._gameDiamond.ToString();
        GetText((int)Texts.MoneyText).text = Managers.Player.MyPlayer._gameMoney.ToString();

        LevelStat levelStat;
        Managers.Data.LevelStatDict.TryGetValue(level, out levelStat);
        int totalExp = levelStat.totalEXP;
        float expPersent = Managers.Player.MyPlayer._currentExp / totalExp;
        GetObject((int)GameObjects.PlayerEXPBar).GetComponent<Slider>().value = expPersent;
    }

    public void OnSendChatButton(PointerEventData evt)
    {
        string chatStr = GetObject((int)GameObjects.ChatContentField).GetComponent<TMP_InputField>().text;

        if (string.IsNullOrEmpty(chatStr))
            return;

        C_SendChat sPkt = new C_SendChat();
        sPkt.messageType = (int)Define.ChatType.Channel;
        sPkt.nickName = Managers.Player.MyPlayer.NickName;
        sPkt.chatContent = chatStr;
        Debug.Log("[NetworkManager] SEND : C_SendChat");
        Managers.Net.Send(sPkt.Write());

        GetObject((int)GameObjects.ChatContentField).GetComponent<TMP_InputField>().text = "";
    }

    public void OnSkillButton(PointerEventData evt)
    {
        Managers.UI.ShowPopupUI<UI_SkillPopup>();
    }

    public void OnChatAdd(object obj)
    {
        ChatPiece chatPiece = obj as ChatPiece;
        GameObject go = Managers.Resource.Instantiate("UI/SubItem/UI_ChatItem", GetObject((int)GameObjects.ChatDashBoardObject).transform);
        go.GetComponent<UI_ChatItem>().SetChatPiece(chatPiece._chatType, chatPiece._nickName, chatPiece._chatContent);
    }

    public void OnAllNoticeAdd(object obj)
    {

    }


}
