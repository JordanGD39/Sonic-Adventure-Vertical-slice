using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCamera : NormalCameraPosition
{
    [SerializeField]
    private Vector3[] _offsetList;

    [SerializeField]
    private AnimationTriggers _player;

    private int offsetCounter;
    private int collisionCounter;

    protected override void Start()
    {
        base.Start();

        offsetCounter = 0;
        collisionCounter = 0;
    }

    protected override void Update()
    {
        if (!stop)
        {
            float horizontal = Input.GetAxis(Constants.Inputs.mouseX);

            _offset = Quaternion.AngleAxis(horizontal * rotationSpeed, Vector3.up) * _offset;

            /*if (offsetCounter != 3)
            {
                _offset = _offsetList[offsetCounter];*/
                transform.position = _target.position + _offset;
            /*}
            else
            {
                transform.position = _offset;
            }*/
        }

        base.Update();

        /*if (_player.Triggered)
        {
            collisionCounter++;
            ChangeOffset(collisionCounter);
        }
        else
        {
            collisionCounter = 0;
        }*/
    }

    /*private void ChangeOffset(int number)
    {
        if (number == 1 && offsetCounter < (_offsetList.Length - 1))
        {
            offsetCounter++;
        }
    }*/
}
