using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        GetDirInput();

        base.UpdateController();
    }

    void GetDirInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
        }
        else
        {
            Dir = MoveDir.None;
        }

        if (Input.GetKey(KeyCode.Space))
        {

        }
    }

    protected void CheckUpdatedFlag()
    {
        if (_isUpdated)
        {
            C_Move movePacket = new C_Move();
            //movePacket.PosInfo = PosInfo;
            Managers.Net.Send(movePacket.Write());
            _isUpdated = false;
        }
    }
}
