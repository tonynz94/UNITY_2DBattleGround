using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Define.CameraMode _mode = Define.CameraMode.AliveMode;
    [SerializeField] GameObject _targetPoint = null;

    public void setTargetPoint(GameObject player)
    {
        _targetPoint = player;
    }
    public void MyPlayerDie()
    {
        _mode = Define.CameraMode.DieMode;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //현재 살아남아서 플레이 중 일때
        switch(_mode)
        {
            case Define.CameraMode.AliveMode:
                if(_targetPoint != null)
                    transform.position = new Vector3(_targetPoint.transform.position.x, _targetPoint.transform.position.y, -10);
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
