using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : UI_Base
{
    public override bool Init()
    {

        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject);
        return true;

        
    }
}
