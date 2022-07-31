using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterItem : UI_Base
{
    public bool _ready { get; private set; } = false;
    public bool _isOwner { get; private set; } = false;
    public bool _isSlotEmpty { get; private set; } = true;
    public int _CGUID { get; private set; } = 0;

    enum Texts
    {
        NickNameText
    }

    enum Objects
    {
        OnCharacterObject,
        OffCharacterObject,
        MeObject,
        OwnerObject,
        ReadyObject,
    }

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindText(typeof(Texts));
        ReadyOff();
        return true;
    }

    public void PlayerEnter(int CGUID ,bool isMe, bool isPlayerReady, bool isOwner = false)
    {
        _isOwner = isOwner;
        _isSlotEmpty = false;
        _CGUID = CGUID;

        GetObject((int)Objects.MeObject).SetActive(isMe);
        GetObject((int)Objects.ReadyObject).SetActive(isPlayerReady);
        GetObject((int)Objects.OwnerObject).SetActive(isOwner);

        GetObject((int)Objects.OnCharacterObject).SetActive(true);
        GetObject((int)Objects.OffCharacterObject).SetActive(false);
        GetText((int)Texts.NickNameText).text = Managers.Player.GetPlayer(CGUID).NickName; 
    }

    public void PlayerLeave()
    {
        GetObject((int)Objects.OnCharacterObject).SetActive(false);
        GetObject((int)Objects.OffCharacterObject).SetActive(true);

        _isSlotEmpty = true;
        _ready = false;
        _isOwner = false;
        _CGUID = 0;
    }

    public bool ReadyOn()
    {
        //서버로 보내야됨
        _ready = true;

        if (_isSlotEmpty == false)
            Managers.Player.GetPlayer(_CGUID)._isPlayerReady = _ready;

        GetObject((int)Objects.ReadyObject).SetActive(true);
        return true;
    }

    public bool ReadyOff()
    {
        //서버로 보내야됨
        _ready = false;

        if (_isSlotEmpty == false)
            Managers.Player.GetPlayer(_CGUID)._isPlayerReady = _ready;

        GetObject((int)Objects.ReadyObject).SetActive(false);
        return false;
    }


}
