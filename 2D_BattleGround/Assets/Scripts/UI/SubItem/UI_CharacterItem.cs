using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterItem : UI_Base
{
    public bool _ready { get; private set; } = false;
    public bool _isOwner { get; private set; } = false;
    public bool _isSlotEmpty { get; private set; } = true;

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

        GetObject((int)Objects.MeObject).SetActive(isMe);
        GetObject((int)Objects.ReadyObject).SetActive(isPlayerReady);
        GetObject((int)Objects.OwnerObject).SetActive(isOwner);

        GetObject((int)Objects.OnCharacterObject).SetActive(true);
        GetObject((int)Objects.OffCharacterObject).SetActive(false);
        GetText((int)Texts.NickNameText).text = Managers.Player.GetPlayer(CGUID).NickName; 
    }

    public void PlayerLeave()
    {
        _isSlotEmpty = true;
        GetObject((int)Objects.OnCharacterObject).SetActive(false);
        GetObject((int)Objects.OffCharacterObject).SetActive(true);
    }

    public bool ReadyOn()
    {
        //서버로 보내야됨
        _ready = true;
        GetObject((int)Objects.ReadyObject).SetActive(true);
        return true;
    }

    public bool ReadyOff()
    {
        //서버로 보내야됨
        _ready = false;
        GetObject((int)Objects.ReadyObject).SetActive(false);
        return false;
    }


}
