using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PlayerController : BaseController
{
    [SerializeField]
    ObjectState _state = Define.ObjectState.Idle;
    [SerializeField]
    protected MoveDir _dir = Define.MoveDir.Down;
    protected MoveDir _lastDir = Define.MoveDir.Down;

    Animator _anim;

    protected MoveDir Dir
    {
        get { return _dir; }
        set {
            _dir = value;
        }
    }


    [SerializeField]
    float _speed = 5f;

    protected Define.ObjectState State
    {
        get{
            return _state;
        }
        set
        {
            if (_state == value)
                return;


            UpdateAnimation();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(State)
        {
            case ObjectState.Idle:
                UpdateIdle();
                break;
            case ObjectState.Moving:
                UpdateMoving();
                break;
         
        }
    }

    void UpdateIdle()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.W))
        {
            Dir = MoveDir.Up;
            transform.Translate(Vector3.up * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Dir = MoveDir.Down;
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Dir = MoveDir.Left;
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Dir = MoveDir.Right;
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        else
        {
            Dir = MoveDir.None;
        }
#else

#endif
    }

    void UpdateMoving()
    {
        if (Dir == MoveDir.None)
            return;

        State = ObjectState.Moving;
    }

    public void UpdateAnimation()
    {
        if(State == ObjectState.Idle)
        {
            switch(_lastDir)
            {
                case MoveDir.Up:
                    _anim.Play("IDLE_UP");
                    break;
                case MoveDir.Down:
                    _anim.Play("IDLE_DOWN");
                    break;
                case MoveDir.Left:
                    _anim.Play("IDLE_LEFT");
                    break;
                case MoveDir.Right:
                    _anim.Play("IDLE_RIGHT");
                    break;
            }
        }
        else if(State == ObjectState.Moving)
        {
            switch(Dir)
            {
                case MoveDir.Up:
                    _anim.Play("WALK_UP");
                    break;
                case MoveDir.Down:
                    _anim.Play("WALK_DOWN");
                    break;
                case MoveDir.Left:
                    _anim.Play("WALK_LEFT");
                    break;
                case MoveDir.Right:
                    _anim.Play("WALK_RIGHT");
                    break;
            }
        }

    }
}
