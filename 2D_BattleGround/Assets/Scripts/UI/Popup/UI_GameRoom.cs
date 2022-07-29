using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameRoom : UI_Popup
{
    //�� ID
    int _roomID;

    //�� �ȿ� �÷��̾�
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

    //�� �������� �� �Լ��� �����ϰ� ��
    public void CreateRoom(int roomID, Player ownerPlayer)
    {
        _roomID = roomID;
        playerList.Add(ownerPlayer._CGUID, ownerPlayer);
        GameRoom room = Managers.Room.GetGameRoom(_roomID);
        room.AddPlayer(ownerPlayer._CGUID);

        RefreashSlot();
        RefreashTitle();
        RefreashMap();
    }

    
    public void EnterRoom(int roomId, int CGUID)
    {
        _roomID = roomId;

        GameRoom room = Managers.Room.GetGameRoom(_roomID);
        room.AddPlayer(CGUID);
        playerList.Add(CGUID, Managers.Player.GetPlayer(CGUID));
        

        RefreashSlot();
        RefreashTitle();
        RefreashMap();
    }

    public void OnReadyButton()
    {
        bool ready = Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_1)._ready;
        C_ClickReadyOnOff sPkt = new C_ClickReadyOnOff();
        sPkt.CGUID = Managers.Player.GetMyCGUID();
        sPkt.roomId = _roomID;

        if (ready)  //true
        {
            //���� ���������
            GetText((int)Texts.ReadyButtonText).text = "READY";
            GetButton((int)Buttons.ReadyButton).GetComponent<Image>().color = _readyColor;
            Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_1).ReadyOff();
            sPkt.isReady = false;
        }
        else
        {
            //���� ������.
            GetText((int)Texts.ReadyButtonText).text = "CANCEL";
            GetButton((int)Buttons.ReadyButton).GetComponent<Image>().color = _cancelColor; ;
            Get<UI_CharacterItem>((int)CharacterItem.UI_CharacterItem_1).ReadyOn();
            sPkt.isReady = true;
        }

        Debug.Log($"[NetworkManager] @>> SEND : C_ClickReadyOnOff Ready - { sPkt.isReady } ");
        Managers.Net.Send(sPkt.Write());
    }

    public void OnBackButton()
    {
        Managers.UI.ClosePopupUI();
        C_GameToLobby cPkt = new C_GameToLobby();
        cPkt.CGUID = Managers.Player.GetMyCGUID();
        cPkt.roomId = _roomID;

        Debug.Log("[NetworkManager] SEND : C_GameToLobby");
        Managers.Net.Send(cPkt.Write());
        Managers.Room.GameRoomAllClear();
        Debug.Log("Back Clicked");
    }

    public void RefreashSlot()
    {
        int i = 1;
        GameRoom room = Managers.Room.GetGameRoom(_roomID);
        foreach(Player player in room._playerDic.Values)
        {
            UI_CharacterItem item = Get<UI_CharacterItem>((int)System.Enum.Parse(typeof(CharacterItem), $"UI_CharacterItem_{i}"));
            item.PlayerEnter(
                CGUID        : player._CGUID, 
                isMe         : player._CGUID == Managers.Player.GetMyCGUID(), 
                isPlayerReady: player._isPlayerReady, 
                isOwner      : player._CGUID == room.roomOwner
            );
            i++;
        }

        for( ; i<=4; i++)
            Get<UI_CharacterItem>((int)System.Enum.Parse(typeof(CharacterItem), $"UI_CharacterItem_{i}")).PlayerLeave();   
    }

    public void RefreashTitle()
    {
        GetText((int)Texts.TitleText).text = $"Match Lobby ({Managers.Room.GetGameRoom(_roomID).GetPlayerCount()}/4)";
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

    //���� ������ ��
    public void LeaveRoom()
    {

    }

    //�濡 �ٸ� ������ ������ ��
    public void LeaveRoom(int CGUID)
    {

    }

    public void SetMyCharacterSlot()
    {
        
    }

    public void SetReady()
    {

    }
}
