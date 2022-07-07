using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : ObjectController
{
    [SerializeField]
    Define.ObjectState _state = Define.ObjectState.None;

    [SerializeField]
    float _speed = 5f;

    protected Define.ObjectState State
    {
        get{
            return _state;
        }
        set
        {

        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up *_speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
#else

#endif
    }
}
