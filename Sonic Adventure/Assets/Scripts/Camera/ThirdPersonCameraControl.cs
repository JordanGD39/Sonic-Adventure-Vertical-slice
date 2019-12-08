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
        _target = GameObject.FindGameObjectWithTag(Constants.Tags.player).transform;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        base.Start();
        offsetMagnitude = _offset.magnitude;

        wallHit = false;
        thenOffset = _offset.magnitude;
    }

    protected override void Update()
    {
        if (_player.WallHit)
        {
            _offset = _player.Hit.point - _player.PlayerTransform.position + (transform.forward * 0.1f);
        }

        base.Update();

        if (!wallHit)
        {
            float currentDistance = thenOffset - Vector3.Distance(transform.position, _target.position);

            if (currentDistance > 0.05f || currentDistance < -0.05f)
            {
                transform.position = _cameraPositionReference.position;
            }
        }

        transform.LookAt(_target);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != Constants.Tags.player && collision.gameObject.tag != Constants.Tags.item)
        {
            wallHit = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag != Constants.Tags.player && collision.gameObject.tag != Constants.Tags.item)
        {
            wallHit = false;
        }
    }
}