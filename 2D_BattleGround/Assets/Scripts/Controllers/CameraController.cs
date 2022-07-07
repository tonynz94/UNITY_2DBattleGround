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
        //���� ��Ƴ��Ƽ� �÷��� �� �϶�
        switch(_mode)
        {
            case Define.CameraMode.AliveMode:
                break;
            case Define.CameraMode.DieMode:
                break;
            case Define.CameraMode.EndingMode:
                break;
            case Define.CameraMode.GameOverMode:
                break;
        }
    }
}
