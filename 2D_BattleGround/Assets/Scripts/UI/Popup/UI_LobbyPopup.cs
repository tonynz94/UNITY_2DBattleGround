using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LobbyPopup : UI_Popup
{
    enum Buttons
    {
        StartButton,
        HeroButton,
    }

    enum Texts
    {
        StartButtonText,
        HeroButtonText,
        PlayerNickText,
        PlayerLevelText,
        MoneyText,
        DiamondText,

    }

    enum Images
    {
        ProfileImage,
        PlayerAvatorImage,
    }

    enum GameObjects
    {
        PlayerEXPBar
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));
        BindImage(typeof(Images));
        BindObject(typeof(GameObjects));

        BindEvent(GetButton((int)Buttons.StartButton).gameObject, OnStartButton);
        BindEvent(GetButton((int)Buttons.HeroButton).gameObject, OnHeroButton);

        InitPlayerInfo();

        return true;
    }

    public void OnStartButton()
    {
        Debug.Log("Play Game");
    }

    public void OnHeroButton()
    {
        Debug.Log("My Hero List");
    }

    public void InitPlayerInfo()
    {
        GetText((int)Texts.PlayerNickText).text = Managers.Player.NickName;
        GetText((int)Texts.PlayerLevelText).text = Managers.Player.Level.ToString();
        GetText((int)Texts.DiamondText).text = Managers.Player._gameDiamond.ToString();
        GetText((int)Texts.MoneyText).text = Managers.Player._gameMoney.ToString();
    }
}
