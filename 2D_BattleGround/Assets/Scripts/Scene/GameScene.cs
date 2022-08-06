using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.Map.LoadMap(1);
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_IngameBGM");

        C_EnterFieldWorld cPkt = new C_EnterFieldWorld();
        cPkt.CGUID = Managers.Player.GetMyCGUID();

        Managers.Net.Send(cPkt.Write());

        //GameObject player =Managers.Game.Spawn(Define.WorldObject.Player, "Objects/MyPlayer");
        //Camera.main.gameObject.AddComponent<CameraController>().TargetPoint(player);

        return true;
    }

}
