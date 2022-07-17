using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterItem : UI_Base
{
    public bool _ready { get; private set; } = false;
    public bool _isOwner { get; private set; } = false;
    enum Objects
    {
        OnCharacterObject,
        OffCharacterObject,
        MeObject,
        ReadyObject
    }

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        ReadyOff();
        return true;
    }

    public void PlayerEnter(bool isOwner = false)
    {
        _isOwner = isOwner;

        GetObject((int)Objects.OnCharacterObject).SetActive(true);
        GetObject((int)Objects.OffCharacterObject).SetActive(false);

        GetObject((int)Objects.ReadyObject).SetActive(_isOwner);
    }

    public void PlayerLeave()
    {
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
