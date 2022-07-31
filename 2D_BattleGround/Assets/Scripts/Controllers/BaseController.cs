using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseController : MonoBehaviour
{
    protected Animator _anim;
    [SerializeField]
    public Define.WorldObject WorldObjectType { get; private set; } = Define.WorldObject.Unknown;

    [SerializeField]
    protected ObjectState _state = Define.ObjectState.Idle;
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

    protected MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_dir == value)
                return;

            _dir = value;
            if (value != MoveDir.None)
                _lastDir = value;

            UpdateAnimation();
        }
    }


    [SerializeField]
    protected float _speed = 5f;

    protected Define.ObjectState State
    {
        get
        {
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

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateController();
    }

    protected virtual void Init()
    {
        _anim = GetComponent<Animator>();
    }

    protected virtual void UpdateController()
    {

    }

    protected virtual void UpdateIdle()
    {

    }

    protected virtual void UpdateMoving()
    {

    }

    protected virtual void UpdateDead()
    {

    }

    protected virtual void UpdateAnimation()
    {

    }

}
