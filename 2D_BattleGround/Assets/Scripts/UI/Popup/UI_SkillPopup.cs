using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillPopup : UI_Popup
{
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
        BindButton(typeof(Images));
        BindButton(typeof(Texts));

        BindEvent(GetButton((int)Buttons.HomeButton).gameObject, OnHomeButton);
        BindEvent(GetButton((int)Buttons.HomeButton).gameObject, OnSpeedUpSkillButton);
        BindEvent(GetButton((int)Buttons.HomeButton).gameObject, OnRangeUpSkillButton);
        BindEvent(GetButton((int)Buttons.HomeButton).gameObject, OnPowerUpSkillButton);
        BindEvent(GetButton((int)Buttons.HomeButton).gameObject, OnWaterCountUpSkillButton);


        return true;
    }

    private void Start()
    {
        
    }

    public void OnSpeedUpSkillButton()
    {

    }

    public void OnRangeUpSkillButton()
    {

    }

    public void OnPowerUpSkillButton()
    {

    }

    public void OnWaterCountUpSkillButton()
    {

    }

    public void OnHomeButton()
    {

    }

    public void RefreshSkillUI()
    {

    }
}
