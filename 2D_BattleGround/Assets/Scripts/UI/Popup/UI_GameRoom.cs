using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameRoom : UI_Popup
{
    //방 ID
    int _roomID;
    int _playerCount = 0;
    Dictionary<int, UI_CharacterItem> slot = new Dictionary<int, UI_CharacterItem>();

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
        StartButton,
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

        BindEvent(GetButton((int)Buttons.StartButton).gameObject, OnStartButton);
        BindEvent(GetButton((int)Buttons.ReadyButton).gameObject, OnReadyButton);
        BindEvent(GetButton((int)Buttons.BackButton).gameObject, OnBackButton);

        return true;
    }

    //방 만들사람만 이 함수를 실행하게 됨
    public void CreateRoom(int roomID, Player ownerPlayer)
    {
        _roomID = roomID;
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

        RefreashSlot();
        RefreashTitle();
        RefreashMap();
    }

    
    public void OnHandleReadyActiive(S_ClickReadyOnOff sPkt)
    {
        UI_CharacterItem item;
        slot.TryGetValue(sPkt.CGUID, out item);
        if (item == null)
            Debug.LogError("해당 CGUID는 방에 없어요.");
        
        if (sPkt.isReady)  //true
        {
            item.ReadyOn();
            if (sPkt.CGUID == Managers.Player.GetMyCGUID())
            {
                GetText((int)Texts.ReadyButtonText).text = "CANCEL";
                GetButton((int)Buttons.ReadyButton).GetComponent<Image>().color = _cancelColor;
            }
        }
        else
        {
            item.ReadyOff();
            if (sPkt.CGUID == Managers.Player.GetMyCGUID())
            {
                GetText((int)Texts.ReadyButtonText).text = "READY";
                GetButton((int)Buttons.ReadyButton).GetComponent<Image>().color = _readyColor;
            }    
        }
    }

    //방장 일때.
    public void OnStartButton(PointerEventData evt)
    {
        C_GameStart cPkt = new C_GameStart();
        if(Managers.Player.GetMyCGUID() == Managers.Room.GetGameRoom(_roomID).roomOwner)
        {
            cPkt.CGUID = Managers.Player.GetMyCGUID();
            cPkt.roomID = _roomID;

            Managers.Net.Send(cPkt.Write());
        }
        else
        {
            Debug.LogError("You'r not owner");
        }
    }

    //이 레디함수는 내가 누를때만 실행됨.
    public void OnReadyButton(PointerEventData evt)
    {
        C_ClickReadyOnOff sPkt = new C_ClickReadyOnOff();

        UI_CharacterItem item;
        slot.TryGetValue( Managers.Player.GetMyCGUID() , out item);
        if (item == null)
            Debug.LogError("해당 CGUID는 방에 없어요.");

        //만약 레디에서 레디버튼을 눌렀다면  isReady false가 보내진다.
        sPkt.CGUID = Managers.Player.GetMyCGUID();
        sPkt.roomId = _roomID;
        sPkt.isReady = !item._ready;
        Managers.Net.Send(sPkt.Write());
    }

    public void OnBackButton(PointerEventData evt)
    {
        C_GameToLobby cPkt = new C_GameToLobby();
        cPkt.CGUID = Managers.Player.GetMyCGUID();
        cPkt.roomId = _roomID;

        Debug.Log("[NetworkManager] SEND : C_GameToLobby");
        Managers.Net.Send(cPkt.Write());
    }

    public void RefreashSlot()
    {
        int i = 1;
        slot.Clear();

        GameRoom room = Managers.Room.GetGameRoom(_roomID);

        GetButton((int)Buttons.StartButton).gameObject.SetActive(Managers.Player.GetMyCGUID() == room.roomOwner);
        GetButton((int)Buttons.ReadyButton).gameObject.SetActive(Managers.Player.GetMyCGUID() != room.roomOwner);

        foreach(Player player in room._playerList)
        {
            UI_CharacterItem item = Get<UI_CharacterItem>((int)System.Enum.Parse(typeof(CharacterItem), $"UI_CharacterItem_{i}"));
            slot.Add( player._CGUID, item);
            item.PlayerEnter(
                CGUID          : player._CGUID, 
                isMe             : player._CGUID == Managers.Player.GetMyCGUID(), 
                isPlayerReady : player._isPlayerReady, 
                isOwner        : player._CGUID == room.roomOwner
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

    //내가 나갔을 때
    public void LeaveRoom()
    {

    }

    //방에 다른 유저가 나갔을 때
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
