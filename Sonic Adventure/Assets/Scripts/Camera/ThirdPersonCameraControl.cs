using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    [SerializeField]
    private Vector3 _offset;

    [SerializeField]
    private PlayerCameraRelation _player;

    private float rotationSpeed = 3.0f;
    private float offsetMagnitude;

    private bool stop = false;

    public bool Stop { get { return stop; } set { stop = value;} }

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag(Constants.Tags.player).transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        transform.position = _target.position + _offset;
        offsetMagnitude = _offset.magnitude;
    }

    void FixedUpdate()
    {
        float turnHorizontal = Input.GetAxis("Mouse X");

        if (_player.WallHit)
        {
            _offset = _player.Hit.point - _player.PlayerTransform.position + (transform.forward * 0.1f);
        }

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;

        if (!stop)
        {
            transform.position = _target.position + _offset;
        }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != Constants.Tags.player)
        {
            Debug.Log("Leaving...");
            //_offset -= (transform.forward * 0.1f);
        }
    }
}