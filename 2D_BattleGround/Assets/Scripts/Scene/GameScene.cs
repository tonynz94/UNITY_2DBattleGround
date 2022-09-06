using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Map.LoadMap(3);
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_IngameBGM");
        Camera.main.transform.position = new Vector3(0, 0, -10);

        C_EnterFieldWorld cPkt = new C_EnterFieldWorld();
        cPkt.CGUID = Managers.Player.GetMyCGUID();
        cPkt.roomID = Managers.Game.GetCurrentRoomID();
        Managers.Net.Send(cPkt.Write());

       

        return true;
    }
}
