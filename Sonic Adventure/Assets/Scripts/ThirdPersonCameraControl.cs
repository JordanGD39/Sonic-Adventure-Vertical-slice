using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    private float rotationSpeed = 3.0f;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 _offset;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag(Constants.Tags.playerTag).transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis(Constants.Inputs.mouseX);

        _offset = Quaternion.AngleAxis(moveHorizontal * rotationSpeed, Vector3.up) * _offset;
        transform.position = _target.position + _offset;

        /*Vector3 desiredPosition = _target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;*/

        transform.LookAt(_target);
    }
}