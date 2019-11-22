using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    const float rotationSpeed = 3.0f;

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _smoothSpeed;

    [SerializeField]
    private Vector3 _offset;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        /*RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.blue);
            //Debug.Log(hit.collider.tag);

            float step = _smoothSpeed * Time.deltaTime;

            if (hit.collider.gameObject.tag != "Player")
            {
                transform.position = Vector3.MoveTowards(transform.position, transform.forward, step);
            }
            else if (hit.collider.gameObject.tag == "Player" && hit.distance < 7.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, -transform.forward, step);
            }
        }*/
    }

    void FixedUpdate()
    {
        float turnHorizontal = Input.GetAxis("Mouse X");

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;
        transform.position = _target.position + _offset;

        transform.LookAt(_target);
    }
}