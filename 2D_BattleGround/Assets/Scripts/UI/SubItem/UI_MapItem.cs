using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MapItem : UI_Base
{
    [SerializeField]
    Define.MapType _mapType;

    enum Images{
        MapImageBG,
    }

    enum Texts
    {
        MapImageText,
    }

    enum Buttons
    {
        MapItemButton
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindImage(typeof(Images));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.MapItemButton).gameObject, OnMapItemButton);

        return true;
    }

    public void SetInfo(Define.MapType mapType)
    {
        _mapType = mapType;
    }

    public void OnMapItemButton(PointerEventData evt)
    {
        switch (_mapType)
        {
            case Define.MapType.GrassMap:
                break;
            case Define.MapType.LakeMap:
                break;
            case Define.MapType.TreeMap:
                break;
        }

    }
}
