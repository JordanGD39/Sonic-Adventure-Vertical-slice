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
        _target = GameObject.FindGameObjectWithTag(Constants.Tags.player).transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = _target.position + _offset;
        stop = false;
    }

    virtual protected void Update()
    {
        //_offset = Quaternion.AngleAxis((_target.rotation.y - transform.rotation.y) * rotationSpeed, Vector3.up) * _offset;
        //_offset = Quaternion.AngleAxis((_target.rotation.z - transform.rotation.z) * rotationSpeed, Vector3.right) * _offset;

        transform.LookAt(_target);
    }
}
