using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    const float rotationSpeed = 3.0f;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 _offset;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        float turnHorizontal = Input.GetAxis("Mouse X");

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;
        transform.position = _target.position + _offset;

        transform.LookAt(_target);
    }
}