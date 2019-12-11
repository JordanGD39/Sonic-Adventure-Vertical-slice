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

    public Vector3 Offset { get { return _offset; } set { _offset = value; } }

    protected float rotationSpeed = 3.0f;
    protected bool stop;

    public bool Stop { get { return stop; } set { stop = value; } }

    virtual protected void Start()
    {
        _target = GameObject.FindGameObjectWithTag(Constants.Tags.player).transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = _target.position + _offset;
        stop = false;
    }

    virtual protected void Update()
    {
        //Preventing Gimbal lock when dying
        if (transform.rotation.eulerAngles.x < 89 || !GameManager.instance.Dying)
        {
            transform.LookAt(_target);
        }
    }
}
