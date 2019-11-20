using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    const float rotationSpeed = 30.0f;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 _offset;

    private float turnXAxis;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        turnXAxis = 0.0f;
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Mouse X");

        if (turnXAxis > _offset.z || turnXAxis < -_offset.z)
        {
            turnXAxis -= moveHorizontal;
        }
        else
        {
            //turnXAxis -= moveHorizontal;
        }
    }

    void FixedUpdate()
    {
        //Vector3 desiredPosition = _target.position + _offset;
        Vector3 desiredPosition = _target.position + new Vector3(turnXAxis, _offset.y, _offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed);
        transform.position = smoothedPosition;

        //transform.Translate(new Vector3(-moveHorizontal, 0.0f, 0.0f) * 25.0f * Time.deltaTime);
        //transform.Translate(Vector3.right * 25.0f * Time.deltaTime);

        transform.LookAt(_target);
    }
}