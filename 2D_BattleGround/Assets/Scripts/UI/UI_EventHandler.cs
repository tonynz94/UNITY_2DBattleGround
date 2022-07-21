using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler
{
    public Action OnClickHandler = null;
    public Action OnBeginDragHandler = null;
    public Action OnDragHandler = null;
    //?????? ????????
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke();
    }

    //?? ???? ???????? ???????? ???? ??????
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0f), 0.1f);
        //Managers.Sound.Play(Define.Sound.Effect, "Sound_Bubble");
        if (OnClickHandler != null)
            OnClickHandler.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }
}
