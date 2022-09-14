using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_Controller : UI_Scene
{
    RectTransform _JoyStickLeverObjectRect;
    RectTransform _JoyStickObjectRect;

    MyPlayerController _myPlayerController;
    MoveDir _tempDir = MoveDir.None;

    float _leverRange = 150f;

    enum Objects
    {
        JoyStickLeverObject,
        JoyStickObject,
    }

    enum Buttons
    {
        WaterBoomButton,
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        BindEvent(GetObject((int)Objects.JoyStickLeverObject), OnJoyStickBeginDrag, Define.UIEvent.BeginDrag);
        BindEvent(GetObject((int)Objects.JoyStickLeverObject),OnJoyStickDrag, Define.UIEvent.Drag);
        BindEvent(GetObject((int)Objects.JoyStickLeverObject),OnJoyStickEndDrag, Define.UIEvent.EndDrag);

        BindEvent(GetButton((int)Buttons.WaterBoomButton).gameObject , OnWaterBoomButton);

        _JoyStickLeverObjectRect = GetObject((int)Objects.JoyStickLeverObject).GetComponent<RectTransform>();
        _JoyStickObjectRect = GetObject((int)Objects.JoyStickObject).GetComponent<RectTransform>();
        _myPlayerController = Managers.Game.GetMyPlayerObject().GetComponent<MyPlayerController>();

        _tempDir = MoveDir.None;

        return true;
    }

    private void Update()
    {
        SetPlayerDirection(_JoyStickLeverObjectRect.transform.localPosition);
    }

    public void OnWaterBoomButton(PointerEventData evt)
    {
        if (_myPlayerController != null)
            _myPlayerController.SetWaterBOOM();
    }

    public void OnJoyStickBeginDrag(PointerEventData evt)
    {
        Vector2 InputPos = evt.position - _JoyStickObjectRect.anchoredPosition;
        Vector2 pos = InputPos.magnitude < _leverRange ? InputPos : InputPos.normalized * _leverRange;
        _JoyStickLeverObjectRect.transform.localPosition = pos;

        SetPlayerDirection(pos.normalized);
    }

    public void OnJoyStickDrag(PointerEventData evt)
    {
        Vector2 InputPos = evt.position - _JoyStickObjectRect.anchoredPosition;
        Vector2 pos = InputPos.magnitude < _leverRange ? InputPos : InputPos.normalized * _leverRange;
        _JoyStickLeverObjectRect.transform.localPosition = pos;

        SetPlayerDirection(pos.normalized);
    }

    public void OnJoyStickEndDrag(PointerEventData evt)
    {
        _JoyStickLeverObjectRect.transform.localPosition = Vector2.zero;
        if(_myPlayerController != null)
            _myPlayerController.Dir = MoveDir.None;
    }

    public void SetPlayerDirection(Vector2 normalizePos)
    {
        bool isPosXBigger = Mathf.Abs(normalizePos.x) > Mathf.Abs(normalizePos.y);

        if (normalizePos.x > 0 && normalizePos.y > 0)
        {
            if (isPosXBigger)
            {
                _tempDir = MoveDir.Right;
            }
            else
            {
                _tempDir = MoveDir.Up;
            }
        }
        else if (normalizePos.x > 0 && normalizePos.y < 0)
        {
            if (isPosXBigger)
            {
                _tempDir = MoveDir.Right;
            }
            else
            {
                _tempDir = MoveDir.Down;
            }
        }
        else if (normalizePos.x < 0 && normalizePos.y > 0)
        {
            if (isPosXBigger)
            {
                _tempDir = MoveDir.Left;
            }
            else
            {
                _tempDir = MoveDir.Up;
            }
        }
        else if (normalizePos.x < 0 && normalizePos.y < 0)
        {
            if (isPosXBigger)
            {
                _tempDir = MoveDir.Left;
            }
            else
            {
                _tempDir = MoveDir.Down;
            }
        }
    }
}
