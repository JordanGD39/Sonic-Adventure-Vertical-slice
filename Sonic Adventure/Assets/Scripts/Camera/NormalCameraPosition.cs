using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCameraPosition : MonoBehaviour
{
    //This Class is bound to the 'CameraPositionReference'-gameobject
    //The angle of the 'PlayerCameraRelation'-script's Raycast will remain the same now

    [SerializeField]
    protected Transform _target;

    [SerializeField]
    protected Vector3 _offset;

    protected float rotationSpeed = 3.0f;
    protected bool stop;

    public bool Stop { get { return stop; } set { stop = value; } }

    virtual protected void Start()
    {
        transform.position = _target.position + _offset;
        stop = false;
    }

    virtual protected void FixedUpdate()
    {
        float turnHorizontal = Input.GetAxis("Mouse X");

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;

        if (!stop)
        {
            transform.position = _target.position + _offset;
        }
    }
}
