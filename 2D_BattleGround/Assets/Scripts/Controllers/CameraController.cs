using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Define.CameraMode _mode = Define.CameraMode.AliveMode;
    [SerializeField] GameObject _targetPoint = null;

    public void TargetPoint(GameObject player)
    {
        _targetPoint = player;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        //현재 살아남아서 플레이 중 일때
        if(_mode == Define.CameraMode.AliveMode)
        {

        }
    }
}
