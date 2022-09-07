using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillPopup : UI_Popup
{
    Player _myPlayer = null;

    Color32 _checkColor = new Color32(120, 236, 72, 255);
    Color _emptyColor = Color.white;
    bool _isChanged = false;

    const int MAX_POINT = 5;

    int _tempSpeedUpSkillPoint;
    int _tempRangeUpSkillPoint;
    int _tempPowerUpSkillPoint;
    int _tempWaterCountUpSkillPoint;

    int _tempSkillPoint;
    int _consumeSkillPoint;

    enum Buttons
    {
        SpeedUpSkillButton,
        RangeUpSkillButton,
        PowerUpSkillButton,
        WaterCountUpSkillButton,
        HomeButton,
    }

    enum Images
    {
        SpeedUpPoint1,
        SpeedUpPoint2,
        SpeedUpPoint3,
        SpeedUpPoint4,
        SpeedUpPoint5,

        RangeUpPoint1,
        RangeUpPoint2,
        RangeUpPoint3,

        PowerUpPoint1,
        PowerUpPoint2,
        PowerUpPoint3,
        PowerUpPoint4,
        PowerUpPoint5,

        WaterCountUpPoint1,
        WaterCountUpPoint2,
        WaterCountUpPoint3,
        WaterCountUpPoint4,
        WaterCountUpPoint5,
    }

    enum Texts
    {
        SkillPointNumberText,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindButton(typeof(Buttons));
        BindImage(typeof(Images));
        BindText(typeof(Texts));

        BindEvent(GetButton((int)Buttons.HomeButton).gameObject, OnHomeButton);
        BindEvent(GetButton((int)Buttons.SpeedUpSkillButton).gameObject, OnSpeedUpSkillButton);
        BindEvent(GetButton((int)Buttons.RangeUpSkillButton).gameObject, OnRangeUpSkillButton);
        BindEvent(GetButton((int)Buttons.PowerUpSkillButton).gameObject, OnPowerUpSkillButton);
        BindEvent(GetButton((int)Buttons.WaterCountUpSkillButton).gameObject, OnWaterCountUpSkillButton);

        _myPlayer = Managers.Player.GetMyPlayer();
        return true;
    }

    private void Start()
    {
        _consumeSkillPoint = 0;

        _tempSkillPoint = _myPlayer._skillPoint;
        GetText((int)Texts.SkillPointNumberText).text = _tempSkillPoint.ToString();
        _tempSpeedUpSkillPoint = _myPlayer._SpeedUpSkillCount;
        _tempRangeUpSkillPoint = _myPlayer._RangeUpSkillCount;
        _tempPowerUpSkillPoint = _myPlayer._PowerUpSkillCount;
        _tempWaterCountUpSkillPoint = _myPlayer._WaterCountUpSkillCount;

        SetSkillPointCountUI("SpeedUp", _tempSpeedUpSkillPoint);
        SetSkillPointCountUI("RangeUp", _tempRangeUpSkillPoint);
        SetSkillPointCountUI("PowerUp", _tempPowerUpSkillPoint);
        SetSkillPointCountUI("WaterCountUp", _tempWaterCountUpSkillPoint);
    }

    public void SetSkillPointCountUI(string skillName , int count)
    {
        for (int i = 1; i <= count; i++)
        {
            Image image = GetImage((int)((Images)System.Enum.Parse(typeof(Images), string.Format("{0}Point{1}",skillName, i))));
            image.color = _checkColor;
        }
    }

    public void OnSpeedUpSkillButton()
    {
        
        if (_tempSkillPoint == 0)
        {
            Managers.UI.ShowPopupUI<UI_CommonPopup>().SetPopupCommon(
            Define.PopupCommonType.YES,
            "No SkillPoint", "There are no Skill Point.\nYou can get Skill Point by Leveling UP"
            );
            return;
        }

        if (_tempSpeedUpSkillPoint + 1 <= MAX_POINT)
        {
            _isChanged = true;
            _tempSkillPoint--;
            _tempSpeedUpSkillPoint += 1;
            _consumeSkillPoint++;
            SetSkillPointCountUI("SpeedUp", _tempSpeedUpSkillPoint);
        }
    }

    public void OnRangeUpSkillButton()
    {
        if (_tempSkillPoint == 0)
        {
            Managers.UI.ShowPopupUI<UI_CommonPopup>().SetPopupCommon(
            Define.PopupCommonType.YES,
            "No SkillPoint", "There are no Skill Point.\nYou can get Skill Point by Leveling UP"
            );
            return;
        }

        if (_tempRangeUpSkillPoint + 1 <= 3)
        {
            _isChanged = true;
            _tempSkillPoint--;
            _tempRangeUpSkillPoint += 1;
            _consumeSkillPoint++;
            SetSkillPointCountUI("RangeUp", _tempRangeUpSkillPoint);
        }
    }

    public void OnPowerUpSkillButton()
    {
        if (_tempSkillPoint == 0)
        {
            Managers.UI.ShowPopupUI<UI_CommonPopup>().SetPopupCommon(
            Define.PopupCommonType.YES,
            "No SkillPoint", "There are no Skill Point.\nYou can get Skill Point by Leveling UP"
            );
            return;
        }

        if (_tempPowerUpSkillPoint + 1 <= MAX_POINT)
        {
            _isChanged = true;
            _tempSkillPoint--;
            _tempPowerUpSkillPoint += 1;
            _consumeSkillPoint++;
            SetSkillPointCountUI("PowerUp", _tempPowerUpSkillPoint);
        }
    }

    public void OnWaterCountUpSkillButton()
    {
        if (_tempSkillPoint == 0)
        {
            Managers.UI.ShowPopupUI<UI_CommonPopup>().SetPopupCommon(
                Define.PopupCommonType.YES,
                "No SkillPoint", "There are no Skill Point.\nYou can get Skill Point by Leveling UP"
            );
            return;
        }

        if (_tempWaterCountUpSkillPoint + 1 <= MAX_POINT)
        {
            _isChanged = true;
            _tempSkillPoint--;
            _tempWaterCountUpSkillPoint += 1;
            _consumeSkillPoint++;
            SetSkillPointCountUI("WaterCountUp", _tempWaterCountUpSkillPoint);
        }
    }

    public void OnHomeButton()
    {
        //TODO 서버에 저장
        if (_isChanged)
        {
            Managers.UI.ShowPopupUI<UI_CommonPopup>().SetPopupCommon(
                Define.PopupCommonType.YESNO,
                "Skill LevelUP", "Are you sure you want to save the points?",
                () =>
                {
                    _myPlayer._SpeedUpSkillCount = _tempSpeedUpSkillPoint;
                    _myPlayer._RangeUpSkillCount = _tempRangeUpSkillPoint;
                    _myPlayer._PowerUpSkillCount = _tempPowerUpSkillPoint;
                    _myPlayer._WaterCountUpSkillCount = _tempWaterCountUpSkillPoint;

                    C_SkillState sPkt = new C_SkillState();
                    sPkt.CGUID = Managers.Player.GetMyCGUID();
                    sPkt.SpeedUpPoint = _tempSpeedUpSkillPoint;
                    sPkt.RangeUpPoint = _tempRangeUpSkillPoint;
                    sPkt.PowerUpPoint = _tempPowerUpSkillPoint;
                    sPkt.WaterCountUpPoint = _tempWaterCountUpSkillPoint;

                    Managers.Net.Send(sPkt.Write());
                    int leftSkillPoint = Managers.Player.MyPlayer._skillPoint - _consumeSkillPoint;
                    if (leftSkillPoint < 0)
                    {
                        Debug.LogError("0이하");
                        return;
                    }
                    Managers.Player.MyPlayer._skillPoint = leftSkillPoint;
                    Managers.UI.ClosePopupUI(this);
                }
            );
        }
        else
        {
            Managers.UI.ClosePopupUI(this);
        }
    }
}
