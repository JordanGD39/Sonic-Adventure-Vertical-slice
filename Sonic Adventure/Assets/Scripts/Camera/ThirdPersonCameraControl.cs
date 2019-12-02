using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    const string PLAYER_TAG = "Player";
    const string ITEM_TAG = "Item";

    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _offset;

    private float rotationSpeed = 3.0f;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = _target.position + _offset;
    }

    void FixedUpdate()
    {
        float turnHorizontal = Input.GetAxis("Mouse X");
        //float turnVertical = Input.GetAxis("Mouse Y");

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;
        //_offset = Quaternion.AngleAxis(turnVertical, Vector3.right) * _offset;


        transform.position = _target.position + _offset;

        /*Collider[] cameraCollision = Physics.OverlapSphere(transform.position, colliderRadius);

        if (cameraCollision.Length <= 0)
        {
            transform.position = _target.position + _offset;
        }
        else if (cameraCollision[0].gameObject.tag == ITEM_TAG)
        {
            transform.position = _target.position + _offset;
        }
        else if (Vector3.Distance(_target.position, transform.position) > _offset.magnitude)
        {
            transform.position = _target.position + _offset;
        }
        else
        {

        }*/

        transform.LookAt(_target);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}