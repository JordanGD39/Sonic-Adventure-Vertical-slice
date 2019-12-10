using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCamera : NormalCameraPosition
{
    [SerializeField]
    private Vector3[] _offsetList;

    [SerializeField]
    private AnimationTriggers _player;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        //float horizontal = Input.GetAxis(Constants.Inputs.mouseX);
        //_offset = Quaternion.AngleAxis(horizontal * rotationSpeed, Vector3.up) * _offset;

        SetOffset(_player.Triggered, _player.CollidingObj);

        if (!stop && _player.CollidingObj.name != Constants.Trigger.name[2])
        {
            transform.position = _target.position + _offset;
        }

        base.Update();
    }

    private void SetOffset(bool trigger, Collider collision)
    {
        if (collision.name == Constants.Trigger.name[0])
        {
            //Slightly tilted camera angle when Sonic goes out of the cave
            _offset = Vector3.MoveTowards(_offset, _offsetList[1], 40.0f * Time.deltaTime);
        }
        else if (collision.name == Constants.Trigger.name[1])
        {
            //Camera turns towards loop direction
            _offset = Vector3.MoveTowards(_offset, _offsetList[2], 8.5f * Time.deltaTime);
        }
        else if (collision.name == Constants.Trigger.name[3])
        {
            //Camera changes back to face bridge direction
            _offset = Vector3.MoveTowards(_offset, _offsetList[2], 60.0f * Time.deltaTime);
        }
        else if (collision.name == Constants.Trigger.name[4])
        {
            //Camera rotates and faces the other way
            if (Vector3.Magnitude(_offsetList[4] - _offset) > 0.5f)
            {
                _offset = Quaternion.AngleAxis(6.5f, Vector3.up) * _offset;
            }
        }
        else if (collision.name == Constants.Trigger.name[2])
        {
            //Camera is located outside the loop
            _offset = _offsetList[3];
            transform.position = _offsetList[3];
        }
        else
        {
            //Regular camera offset
            _offset = _offsetList[0];
        }
    }
}
