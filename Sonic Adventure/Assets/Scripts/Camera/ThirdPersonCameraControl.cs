using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraControl : NormalCameraPosition
{
    [SerializeField]
    private PlayerCameraRelation _player;

    [SerializeField]
    private Transform _cameraPositionReference;

    private float offsetMagnitude;
    private float thenOffset;
    private bool wallHit;

    protected override void Start()
    {
        base.Start();
        offsetMagnitude = _offset.magnitude;

        wallHit = false;
        thenOffset = _offset.magnitude;
    }

    protected override void Update()
    {
        /*if (_player.WallHit)
        {
            _offset = _player.Hit.point - _player.PlayerTransform.position + (transform.forward * 0.3f);
        }*/

        /*if (!wallHit && !stop)
        {
            transform.position = _cameraPositionReference.position;
        }*/

        float turnHorizontal = Input.GetAxis("Mouse X");

        _offset = Quaternion.AngleAxis(turnHorizontal * rotationSpeed, Vector3.up) * _offset;

        if (!stop)
        {
            transform.position = _target.position + _offset;
        }

        base.Update();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer != 9 && collision.gameObject.tag != Constants.Tags.item && collision.gameObject.tag != Constants.Tags.mainCamera && collision.gameObject.tag != Constants.Tags.trigger)
        {
            wallHit = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.layer != 9 && collision.gameObject.tag != Constants.Tags.item && collision.gameObject.tag != Constants.Tags.mainCamera && collision.gameObject.tag != Constants.Tags.trigger)
        {
            wallHit = false;
        }
    }
}