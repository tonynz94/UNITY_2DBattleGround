using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IBeginDragHandler, IDragHandler
{
    public Action OnClickHandler = null;
    public Action OnBeginDragHandler = null;
    public Action OnDragHandler = null;
    //드래그 시작할때
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke();
    }

    //그 이후 드래그가 일어날때 게속 실행됨
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClickHandler != null)
            OnClickHandler.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }
}
