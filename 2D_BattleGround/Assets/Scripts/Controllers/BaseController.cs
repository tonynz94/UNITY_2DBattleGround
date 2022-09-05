using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class PositionInfo
{
    public ObjectState State;
    public MoveDir Dir;
    public MoveDir LastDir;
}

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

    protected bool _isUpdated = false;
    protected GameObject FocusGridObject;

    HPBar _hpBar;

    [SerializeField]
    public Vector3Int _cellPos = Vector3Int.zero;
    public Vector3Int CellPos
    {
        get { return _cellPos; } 
        set 
        {
            if (_cellPos == value)
                return;

            _cellPos = value;
            _isUpdated = true;
        }
    }

    public int _HP;

    public int HP
    {
        get { return _HP; }
        set
        {
            _HP = value;
            UpdateHpBar();
        }
    }

    public Vector3 _destPos;

    public Vector3 DestPos
    {
        get { return _destPos; }
        set 
        {
            if (_destPos == value)
                return;

            _destPos = value;
            transform.position = value;
            _isUpdated = true;
        }
    }

    public MoveDir Dir
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
            _isUpdated = true;
        }
    }


    [SerializeField]
    protected float _speed = 5f;

    public Define.ObjectState State
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
            _isUpdated = true;
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

    private void FixedUpdate()
    {
        FocusGridObject.transform.localPosition = new Vector3(_cellPos.x + 0.5f, _cellPos.y + 0.5f, 0);
    }

    protected virtual void Init()
    {
        _anim = GetComponent<Animator>();
        FocusGridObject = Managers.Resource.Instantiate("Objects/FocusGridObject");
        HP = 200;
        AddHpBar();
    }

    protected void AddHpBar()
    {
        GameObject go = Managers.Resource.Instantiate("UI/WorldSpace/HpBar", transform);
        go.transform.localPosition = new Vector3(0, 1.2f, 0);
        go.name = "HpBar";
        _hpBar = go.GetComponent<HPBar>();
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        if (_hpBar == null)
            return;

        float ratio = 0.0f;
        if (HP > 0)
            ratio = ((float)HP) / 200;

        _hpBar.SetHpBar(ratio);
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

    public virtual void UpdateDead()
    {

    }

    protected virtual void UpdateAnimation()
    {

    }


}
