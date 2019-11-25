using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    const string playerTag = "Player";

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private float _smoothSpeed;

    [SerializeField]
    private Vector3 _offset;

    private RaycastHit[] hit = new RaycastHit[2];
    private float rotationSpeed = 3.0f;
    private float playerCameraDistance;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = _target.position + _offset;
        playerCameraDistance = Vector3.Distance(hit[0].point, transform.position);
    }

    void FixedUpdate()
    {
        float turnHorizontal = Input.GetAxis("Mouse X");

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;
        //transform.position = _target.position + _offset;

        if (Physics.Raycast(transform.position, transform.forward, out hit[0]))
        {
            Debug.DrawRay(transform.position, transform.forward * hit[0].distance, Color.blue);
            //Debug.Log("Front Ray hits " + hit[0].collider.tag);

            /*if (hit[0].collider.gameObject.tag != playerTag)
            {
                transform.position = _target.position + (transform.forward * hit[0].distance);
            }*/
        }

        if (Physics.Raycast(transform.position, -transform.forward, out hit[1]))
        {
            Debug.DrawRay(transform.position, -transform.forward * hit[1].distance, Color.red);
            //Debug.Log("Back Ray hits " + hit[1].collider.tag);
        }

        if (hit[0].collider != null && hit[1].collider == null)
        {
            if (hit[0].collider.gameObject.tag == playerTag)
            {
                transform.position = _target.position + _offset;
            }
            else if (hit[0].distance < playerCameraDistance)
            {
                transform.position += (transform.forward * hit[0].distance);
            }
        }
        else if (hit[0].collider != null && hit[1].collider != null)
        {
            if (hit[0].collider.gameObject.tag == playerTag && hit[1].distance > 0.05f)
            {
                transform.position = _target.position + _offset;
                Debug.Log(playerCameraDistance);
            }
            else if (hit[0].distance < playerCameraDistance)
            {
                transform.position += (transform.forward * hit[1].distance);
            }
        }

        if (hit[0].collider == null)
        {
            Debug.Log("No object visible.");
        }

        transform.LookAt(_target);
    }
}