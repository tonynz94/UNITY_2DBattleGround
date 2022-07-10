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
    //�巡�� �����Ҷ�
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDragHandler != null)
            OnBeginDragHandler.Invoke();
    }

    //�� ���� �巡�װ� �Ͼ�� �Լ� �����
    public void OnDrag(PointerEventData eventData)
    {
        if (OnDragHandler != null)
            OnDragHandler.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0f), 0.1f);
        Managers.Sound.Play(Define.Sound.Effect, "Sound_Bubble");
        if (OnClickHandler != null)
            OnClickHandler.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new NotImplementedException();
    }
}
