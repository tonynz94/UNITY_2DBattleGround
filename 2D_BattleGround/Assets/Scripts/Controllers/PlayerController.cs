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
    [SerializeField]
    protected MoveDir _lastDir = Define.MoveDir.Down;
    [SerializeField]
    protected string currentAni = "";

    [SerializeField]
    public Vector3Int _cellPos = Vector3Int.zero;
    public Vector3Int CellPos 
    { 
        get { return _cellPos; }  
        set { _cellPos = value; } 
    }

    Animator _anim;

    protected MoveDir Dir
    {
        get { return _dir; }
        set {
            if (_dir == value)
                return;

            _dir = value;
            if (value != MoveDir.None)
                _lastDir = value;

            UpdateAnimation();
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

            _state = value;
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
        GetDirInput();

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

    void GetDirInput()
    {
#if UNITY_EDITOR
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
#else

#endif
    }

    void UpdateIdle()
    {
        if(Dir != MoveDir.None)
            State = ObjectState.Moving;
    }

    void UpdateMoving()
    {

        if (Dir == MoveDir.None)
        {
            State = ObjectState.Idle;
            return;
        }
        Vector3 destPos = CellPos;
        switch(Dir)
        {
            case MoveDir.Down:
                destPos = (Vector3.down * _speed * Time.deltaTime) + transform.position;
                break;
            case MoveDir.Left:
                destPos = (Vector3.left * _speed * Time.deltaTime) + transform.position;
                break;
            case MoveDir.Right:
                destPos = (Vector3.right * _speed * Time.deltaTime) + transform.position;
                break;
            case MoveDir.Up:
                destPos = (Vector3.up * _speed * Time.deltaTime) + transform.position;
                break;
        }

        Debug.Log($"목적지 : {destPos}");
        //음수일때는 -0.5 -> -1로 설정되야하고
        //양수일때는 0.5 -> 0로 되어야함 
        Vector3Int destPosInt = new Vector3Int((int)Mathf.Ceil(destPos.x)-1, (int)Mathf.Floor(destPos.y), 0);
        if(Managers.Map.CanGo(destPosInt))
        {
            transform.position = destPos;
            CellPos = destPosInt;
        }
    }

    void UpdateDead()
    {
        Debug.Log("Player Dead");
    }


    public void UpdateAnimation()
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
