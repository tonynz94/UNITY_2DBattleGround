using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_KillAndDeathItem : UI_Base
{
    enum Images
    {
        DeathProfileImage,
        KillerProfileImage
    }

    enum Texts
    {
        DeathNickNameText,
        KillerNickNameText
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindText(typeof(Texts));
        BindImage(typeof(Images));

        return true;
    }

    public void SetKillDeath(KillDeath killDeath)
    {
        Player attackerPlayer = Managers.Player.GetPlayer(killDeath._attackerCGUID);
        Player killedPlayer = Managers.Player.GetPlayer(killDeath._deathCGUID);

        GetText((int)Texts.DeathNickNameText).text = attackerPlayer._nickName;
        GetText((int)Texts.KillerNickNameText).text = killedPlayer._nickName;

        Managers.Resource.Destroy(this.gameObject, 2.0f);
    }
}
