using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class MyPlayerController : PlayerController
{
    bool _waterBoomPressed = false;
    float _startTick = 0f;
    float _movePacketDelayMilSec = 250;
    List<ArraySegment<byte>> _movePacketList = new List<ArraySegment<byte>>();

    protected override void Init()
    {
        base.Init();
        _speed = Managers.Player.GetMyPlayer()._speed;
        _startTick = System.Environment.TickCount;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetWaterBOOM();
        }
    }

    protected void SetWaterBOOM()
    {
        Vector2Int cellPos = new Vector2Int(_cellPos.x, _cellPos.y);
        if (Managers.Game.FindObjectsInField(cellPos) == null)
        {
            C_WaterBOOM cPkt = new C_WaterBOOM();
            cPkt.CGUID = Managers.Player.GetMyCGUID();
            cPkt.roomID = Managers.Game.GetCurrentRoomID();
            cPkt.CellPosX = _cellPos.x;
            cPkt.CellPosY = _cellPos.y;

            Managers.Net.Send(cPkt.Write());
        }
    }

    protected override void UpdateMoving()
    {
        if (Dir == MoveDir.None)
        {
            State = ObjectState.Idle;
            CheckUpdatedFlag();
            return;
        }
        Vector3 destPosTemp = CellPos;
        switch (Dir)
        {
            case MoveDir.Down:
                destPosTemp = (Vector3.down * _speed * Time.deltaTime) + transform.position;
                break;
            case MoveDir.Left:
                destPosTemp = (Vector3.left * _speed * Time.deltaTime) + transform.position;
                break;
            case MoveDir.Right:
                destPosTemp = (Vector3.right * _speed * Time.deltaTime) + transform.position;
                break;
            case MoveDir.Up:
                destPosTemp = (Vector3.up * _speed * Time.deltaTime) + transform.position;
                break;
        }

        Vector3Int cellPosInt = new Vector3Int((int)Mathf.Ceil(destPosTemp.x) - 1, (int)Mathf.Floor(destPosTemp.y), 0);

        if (Managers.Map.CanGo(cellPosInt))
        {
            DestPos = destPosTemp;
            CellPos = cellPosInt;
        }

        CheckUpdatedFlag();
    }

    protected void CheckUpdatedFlag()
    {
        
        if (_isUpdated)
        {
            C_Move movePacket = new C_Move();
            movePacket.roomID = Managers.Game.GetCurrentRoomID();
            movePacket.CGUID = Managers.Player.GetMyCGUID();
            movePacket.posX = _destPos.x;
            movePacket.posY = _destPos.y;
            movePacket.cellPosX = _cellPos.x;
            movePacket.cellPosY = _cellPos.y;
            movePacket.Dir = (int)_dir;
            movePacket.State = (int)_state;

           // if (System.Environment.TickCount < _startTick + _movePacketDelayMilSec)
           // {
           //     _movePacketList.Add(movePacket.Write());
           //     Debug.Log("0.25ÃÊ Àü");
           //     return;
           // }
           //
           // _startTick = System.Environment.TickCount;

            Managers.Net.Send(movePacket.Write());
            _isUpdated = false;
            //_movePacketList.Clear();
        }
    }
}
