using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : BaseController
{
    protected override void Init()
    {
        base.Init();
    }

    protected override void UpdateController()
    {
        switch (State)
        {
            case ObjectState.Idle:
                UpdateIdle();
                break;
            case ObjectState.Moving:
                UpdateMoving();
                break;
            case ObjectState.Dead:
                UpdateDead();
                break;
        }
    }

    protected override void UpdateIdle()
    {
        if(Dir != MoveDir.None)
            State = ObjectState.Moving;
    }

    protected override void UpdateDead()
    {
        Debug.Log("Player Dead");
    }

    protected override void UpdateAnimation()
    {
        if(State == ObjectState.Idle)
        {
            switch(_lastDir)
            {
                case MoveDir.Up:
                    currentAni = "IDLE_UP";
                    _anim.Play("IDLE_UP");
                    break;
                case MoveDir.Down:
                    currentAni = "IDLE_DOWN";
                    _anim.Play("IDLE_FRONT");
                    break;
                case MoveDir.Left:
                    currentAni = "IDLE_LEFT";
                    _anim.Play("IDLE_LEFT");
                    break;
                case MoveDir.Right:
                    currentAni = "IDLE_RIGHT";
                    _anim.Play("IDLE_RIGHT");
                    break; 
            }
        }
        else if(State == ObjectState.Moving)
        {
            switch(Dir)
            {
                case MoveDir.Up:
                    currentAni = "WALK_UP";
                    _anim.Play("WALK_UP");
                    break;
                case MoveDir.Down:
                    currentAni = "WALK_DOWN";
                    _anim.Play("WALK_DOWN");
                    break;
                case MoveDir.Left:
                    currentAni = "WALK_LEFT";
                    _anim.Play("WALK_LEFT");
                    break;
                case MoveDir.Right:
                    currentAni = "WALK_RIGHT";
                    _anim.Play("WALK_RIGHT");
                    break;
            }
        }

    }
}
