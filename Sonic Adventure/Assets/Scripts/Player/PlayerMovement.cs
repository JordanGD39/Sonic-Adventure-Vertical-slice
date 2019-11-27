using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    private PlayerJump playerJump;
    
    private Vector3 movement;
    [SerializeField] private float speed = 3;
    [SerializeField] private float prevRot = 0;
    [SerializeField] private bool loopTime = false;
    [SerializeField] private bool onLoop = false;
    [SerializeField] private bool offTheRamp = false;
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool boosting = false;

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
                //Debug.Log(hit.normal);
                transform.rotation = new Quaternion(quat.x, transform.rotation.y, 0, transform.rotation.w);
                grounded = true;
            }

            if (hit.collider.gameObject.CompareTag(Constants.Tags.boostPad))
            {
                grounded = true;
            }
        }
        else
        {
            grounded = false;
            transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
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
        if (rb.velocity.magnitude > 2.6f && rb.velocity.magnitude <= 14f)
        {
            if (speed < 16)
            {
                speed += 0.5f;
            }                   
        }
        else if (rb.velocity.magnitude > 14f)
        {
            if (speed < 18)
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
                    speed = 18;
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

        if (transform.rotation.x <= 0.35f && transform.rotation.x >= -0.35f)
        {
            Camera.main.transform.GetChild(0).rotation = Camera.main.transform.localRotation;
            if (grounded)
            {
                offTheRamp = false;
            }
            Vector3 tempVect = new Vector3();
            if (!boosting)
            {
                tempVect = Camera.main.transform.TransformVector(movement);
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
        else
        {
            if (!offTheRamp)
            {
                loopTime = true;
                rb.useGravity = false;

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
    }

    public IEnumerator Boost()
    {        
        boosting = true;
        transform.rotation = boostTransform.rotation;
        yield return new WaitForSeconds(boostSec);
        boosting = false;
        playerJump.enabled = true;
    }

    public void BoostPad(float sec, Transform tr)
    {
        boostSec = sec;
        boostTransform = tr;
    }

    public void StartBoost()
    {
        StartCoroutine("Boost");
    }

    public void StopBoost()
    {
        StopCoroutine("Boost");
    }
}


























