using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CreateRoom : UI_Popup
{
    Color32 _SelectColor = new Color32(139, 199, 235, 255);
    Color32 _unSelectColor = new Color32(197, 197, 197, 255);
    public Define.MapType _mapType = Define.MapType.None;
    public Define.GameMode _modeType = Define.GameMode.None;

    GameRoom _gameRoom = null;

    enum Texts
    {
        SubTitleText
    }

    enum Buttons
    {
        MapItemButton_Tree,
        MapItemButton_Lake,
        MapItemButton_Grass,
        MapItemButton_PVP,
        MapItemButton_Coop,
        OKButton,
        BackButton
    }

    enum Images
    {
        UI_MapItem_Tree,
        UI_MapItem_Lake,
        UI_MapItem_Grass,
        UI_ModeItem_PVP,
        UI_ModeItem_Coop,
    }

    enum Objects
    {
        SelectMapObject,
        SelectModeObject
    }


    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindObject(typeof(Objects));
        //Bind<UI_Map>(typeof(MapItems));

        BindEvent(GetButton((int)Buttons.OKButton).gameObject, OnOKButton);
        BindEvent(GetButton((int)Buttons.BackButton).gameObject, OnBackButton);

        BindEvent(GetButton((int)Buttons.MapItemButton_Tree).gameObject, () => OnMapSelect(Define.MapType.TreeMap));
        BindEvent(GetButton((int)Buttons.MapItemButton_Lake).gameObject, () => OnMapSelect(Define.MapType.LakeMap));
        BindEvent(GetButton((int)Buttons.MapItemButton_Grass).gameObject, () => OnMapSelect(Define.MapType.GrassMap));

        BindEvent(GetButton((int)Buttons.MapItemButton_Coop).gameObject, () => OnModeSelect(Define.GameMode.CooperativeMode));
        BindEvent(GetButton((int)Buttons.MapItemButton_PVP).gameObject, () => OnModeSelect(Define.GameMode.PvPMode));


        GetObject((int)Objects.SelectModeObject).SetActive(true);
        GetObject((int)Objects.SelectMapObject).SetActive(false);

        _gameRoom = new GameRoom();

        OnModeSelect(Define.GameMode.PvPMode);
        
        return true;
    }

    public void OnOKButton()
    {

        if (GetObject((int)Objects.SelectModeObject).activeSelf)
        {
            //�� �������� ����
            GetObject((int)Objects.SelectMapObject).SetActive(true);
            GetObject((int)Objects.SelectModeObject).SetActive(false);
            OnMapSelect(Define.MapType.TreeMap);
        }
        else
        {
            //���������� �� ������� 
            Debug.Log($"_modeType : {_modeType} , _mapType : {_mapType}");
            _gameRoom.SetGameRoom(_modeType, _mapType);
            Managers.Room.CreateGameRoom(_gameRoom);

            //������ ����.
            Managers.UI.ClosePopupUI();
            UI_GameRoom room = Managers.UI.ShowPopupUI<UI_GameRoom>();
            room.SetRoom(_gameRoom.roomId, Managers.Player.MyPlayer);
        }
    }

    public void OnBackButton()
    {
        if(GetObject((int)Objects.SelectModeObject).activeSelf)
        {
            Managers.UI.ClosePopupUI();
        }
        else
        {
            GetObject((int)Objects.SelectMapObject).SetActive(false);
            GetObject((int)Objects.SelectModeObject).SetActive(true);
        }
        
    }

    public void OnMapSelect(Define.MapType mapType)
    {
        switch (mapType)
        {
            case Define.MapType.GrassMap:
                GetImage((int)Images.UI_MapItem_Grass).color = _SelectColor;
                GetImage((int)Images.UI_MapItem_Lake).color = _unSelectColor;
                GetImage((int)Images.UI_MapItem_Tree).color = _unSelectColor;
                break;
            case Define.MapType.LakeMap:
                GetImage((int)Images.UI_MapItem_Grass).color = _unSelectColor;
                GetImage((int)Images.UI_MapItem_Lake).color = _SelectColor;
                GetImage((int)Images.UI_MapItem_Tree).color = _unSelectColor;
                break;
            case Define.MapType.TreeMap:
                GetImage((int)Images.UI_MapItem_Grass).color = _unSelectColor;
                GetImage((int)Images.UI_MapItem_Lake).color = _unSelectColor;
                GetImage((int)Images.UI_MapItem_Tree).color = _SelectColor;
                break;
        }
        _mapType = mapType;
    }

    public void OnModeSelect(Define.GameMode modeType)
    {
        switch (modeType)
        {
            case Define.GameMode.PvPMode:
                GetImage((int)Images.UI_ModeItem_PVP).color = _SelectColor;
                GetImage((int)Images.UI_ModeItem_Coop).color = _unSelectColor;
                break;
            case Define.GameMode.CooperativeMode:
                GetImage((int)Images.UI_ModeItem_PVP).color = _unSelectColor;
                GetImage((int)Images.UI_ModeItem_Coop).color = _SelectColor;
                break;
        }
        _modeType = modeType;
    }
}