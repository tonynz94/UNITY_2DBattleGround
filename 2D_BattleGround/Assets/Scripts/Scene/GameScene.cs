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
        Managers.Sound.Play(Define.Sound.Bgm, "Sound_AboveTheTreetops");
        Managers.Game.Spawn(Define.WorldObject.Player, "Objects/Player");

        return true;
    }

}
