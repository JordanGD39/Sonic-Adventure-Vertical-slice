using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    private PlayerJump playerJump;

    private Vector3 movement;
    private Vector3 movementForce;

    [SerializeField] private float speed = 3;
    [SerializeField] private float prevRot = 0;
    [SerializeField] private bool loopTime = false;
    [SerializeField] private bool onLoop = false;
    [SerializeField] private bool offTheRamp = false;
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool boosting = false;
    [SerializeField] private bool clingToGround = false;

    private float boostSec = 0;
    private Transform boostTransform = null;

    public float Speed { get { return speed; }  set { speed = value; } }
    public bool Boosting { get { return boosting; }  set { boosting = value; } }
    public bool Grounded { get { return grounded; } }
    public Vector3 Movement { get { return movement; } set { movement = value; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<PlayerJump>();
    }

    // Update is called once per frame
    private void Update()
    {
        float moveHorizontal = Input.GetAxis(Constants.Inputs.hori);
        float moveVertical = Input.GetAxis(Constants.Inputs.vert);

        if (boosting)
        {
            movement = new Vector3(0, 0, 1);
        }
        else
        {
            movement = new Vector3(moveHorizontal, 0, moveVertical);

            if (!loopTime)
            {
                Vector3 tempVect = transform.position + Camera.main.transform.TransformVector(movement);
                Quaternion rot;
                rot = transform.rotation;
                transform.LookAt(tempVect);
                transform.rotation = new Quaternion(rot.x, transform.rotation.y, 0, transform.rotation.w);
            }
        }

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.3f, -transform.up, out hit, 0.8f))
        {
            if (!hit.collider.isTrigger)
            {
                Quaternion quat = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
                //Debug.Log(transform.rotation);
                transform.rotation = new Quaternion(quat.x, transform.rotation.y, quat.z, transform.rotation.w);
                grounded = true;
                clingToGround = !boosting;
            }

            if (hit.collider.gameObject.CompareTag(Constants.Tags.boostPad))
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
            if (!playerJump.Jumping)
            {
                transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            }
            speed -= 0.6f;
            if (speed < 7)
            {
                speed += 0.6f;
            }
        }

        if (loopTime && grounded)
        {
            Vector3 rotate;

            if (boosting)
            {
                rotate = new Vector3(0, prevRot, 0);
            }
            else
            {
                rotate = new Vector3(0, 0, 0);
            }

            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal) * Quaternion.Euler(rotate.x, rotate.y, rotate.z);
        }
        else
        {
            prevRot = transform.eulerAngles.y;
        }
    }

    void FixedUpdate()
    {
        Acceleration();

        if (transform.rotation.x <= 0.35f && transform.rotation.x >= -0.35f && !playerJump.Jumping && !playerJump.Attacking)
        {
            Camera.main.transform.GetChild(0).rotation = Camera.main.transform.localRotation;
            offTheRamp = false;
            //Debug.Log("Right up");
            Vector3 tempVect = new Vector3();
            if (!boosting)
            {
                Camera.main.transform.GetChild(0).rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, transform.rotation.z, Camera.main.transform.GetChild(0).rotation.w);
                tempVect = Camera.main.transform.GetChild(0).TransformVector(movement);
            }
            else
            {
                tempVect = transform.TransformVector(movement);
            }
            tempVect *= speed;
            tempVect.y = rb.velocity.y;
            rb.velocity = tempVect;
            loopTime = false;
            rb.useGravity = true;
        }
        else if(transform.rotation.x >= 0.35f || transform.rotation.x <= -0.35f && grounded)
        {
            if (!offTheRamp)
            {
                loopTime = true;
                rb.useGravity = false;
                //Debug.Log("Not Right up");
                Vector3 tempVect;
                if (boosting)
                {
                    tempVect = transform.TransformVector(movement);
                }
                else
                {
                    Camera.main.transform.GetChild(0).rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.GetChild(0).rotation.z, Camera.main.transform.GetChild(0).rotation.w);
                    tempVect = Camera.main.transform.GetChild(0).TransformVector(movement);
                }

                tempVect *= speed * 0.75f;
                rb.velocity = tempVect;
            }

            if (speed < 12)
            {
                rb.useGravity = true;
                transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
                offTheRamp = true;
            }
        }
        else if (!grounded && !playerJump.Attacking && !boosting)
        {
            Vector3 tempVect = Camera.main.transform.TransformVector(movement);
            tempVect *= speed;
            tempVect.y = rb.velocity.y;
            loopTime = false;
            rb.useGravity = true;
            rb.velocity = tempVect;
        }
        else if (!grounded && !boosting && playerJump.Attacking && !playerJump.TargetAttack && rb.useGravity)
        {
            Vector3 tempVect = Camera.main.transform.TransformVector(movement);
            tempVect *= speed * 80;
            tempVect.y = rb.velocity.y;
            rb.AddForce(tempVect);
            if (rb.velocity.magnitude > 25)
            {
                Vector3 limitVect = rb.velocity;
                limitVect = Vector3.ClampMagnitude(limitVect, 25);
                rb.velocity = new Vector3(limitVect.x, rb.velocity.y, limitVect.z);                
            }

            if (rb.velocity.y > 15)
            {
                rb.velocity = new Vector3(rb.velocity.x, 15, rb.velocity.z);
            }
        }

        if (clingToGround && !playerJump.Jumping && grounded)
        {
            rb.AddForce(-transform.up * 50);
        }
    }

    private void Acceleration()
    {
        if (rb.velocity.magnitude > 2.6f && rb.velocity.magnitude <= 14f)
        {
            if (speed < 16)
            {
                speed += 0.5f;
            }
        }
        else if (rb.velocity.magnitude > 14f)
        {
            if (speed < 22)
            {
                speed += 0.5f;
            }
            else
            {
                if (boosting)
                {
                    speed -= 1;
                }
                else
                {
                    speed = 22;
                }
            }
        }
        else if (rb.velocity.magnitude <= 2.6f)
        {
            if (speed > 3)
            {
                speed -= 2;
            }
            else if (speed < 3)
            {
                speed = 3;
            }
        }
    }

    public IEnumerator Boost()
    {
        boosting = true;
        playerJump.Jumping = false;
        transform.rotation = boostTransform.rotation;
        yield return new WaitForSeconds(boostSec);
        boosting = false;
        playerJump.enabled = true;
    }

    public void BoostPad(float sec, Transform tr)
    {
        boostSec = sec;
        boostTransform = tr;
        //Not giving parameters to the IEnumerator because when i do it resumes but with a string it starts over when i stop it
        StartCoroutine("Boost");
    }

    public void StopBoost()
    {
        //Stopping the Ienumerator with a string actually stops it and doesn't pause it
        StopCoroutine("Boost");
    }
}
