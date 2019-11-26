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
    private float _smoothSpeed;

    [SerializeField]
    private Vector3 _offset;

    private float colliderRadius;

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

        colliderRadius = (_offset.magnitude * 0.25f);
    }

    void FixedUpdate()
    {
        float turnHorizontal = Input.GetAxis("Mouse X");

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;
        //transform.position = _target.position + _offset;

        Collider[] cameraCollision = Physics.OverlapSphere(transform.position, colliderRadius);

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

        }

        transform.LookAt(_target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, colliderRadius);
    }
}