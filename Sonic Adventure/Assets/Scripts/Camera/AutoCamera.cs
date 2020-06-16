using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCamera : NormalCameraPosition
{
    [SerializeField]
    private Vector3[] _offsetList;

    [SerializeField]
    private AnimationTriggers _player;

    private PlayerMovement playerMovement;

    protected override void Start()
    {
        base.Start();
        playerMovement = _target.GetComponent<PlayerMovement>();
    }

    protected override void Update()
    {
        //float horizontal = Input.GetAxis(Constants.Inputs.mouseX);
        //_offset = Quaternion.AngleAxis(horizontal * rotationSpeed, Vector3.up) * _offset;
        if (!stop)
        {
            SetOffset(_player.Triggered, _player.CollidingObj);

            if (_player.CollidingObj.name != Constants.Trigger.name_2)
            {
                transform.position = _target.position + _offset;
            }
        }       

        base.Update();
    }

    private void SetOffset(bool trigger, Collider collision)
    {
        switch(collision.name)
        {
            case Constants.Trigger.name_0:
                if (_player.CurrentVel.z >= 0.0f)
                {
                    //Slightly tilted camera angle when Sonic goes out of the cave
                    _offset = Vector3.MoveTowards(_offset, _offsetList[1], 40.0f * Time.deltaTime);
                }
                else
                {
                    //Camera angle goes back to normal
                    _offset = Vector3.MoveTowards(_offset, _offsetList[0], 40.0f * Time.deltaTime);
                }
                break;

            case Constants.Trigger.name_1:
                if (_player.CurrentVel.z > 0.0f)
                {
                    //Camera turns towards loop direction
                    _offset = Vector3.MoveTowards(_offset, _offsetList[2], 16.5f * (playerMovement.Speed / playerMovement.MaxSpeed) * Time.deltaTime);
                }
                else if (_player.CurrentVel.x < 0.0f)
                {
                    //Camera angle goes back to before
                    _offset = Vector3.MoveTowards(_offset, _offsetList[1], 16.5f * (playerMovement.Speed / playerMovement.MaxSpeed) * Time.deltaTime);
                }
                break;

            case Constants.Trigger.name_2:
                //Camera is located outside the loop
                _offset = _offsetList[3];
                transform.position = _offsetList[3];
                break;

            case Constants.Trigger.name_3:
                //Camera changes back to face bridge direction
                _offset = Vector3.MoveTowards(_offset, _offsetList[2], 60.0f * Time.deltaTime);
                break;

            default:
                //Regular camera offset
                _offset = _offsetList[0];
                break;
        }
    }
}
