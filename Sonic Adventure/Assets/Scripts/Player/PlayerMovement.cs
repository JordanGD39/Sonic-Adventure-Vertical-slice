using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    
    private Vector3 movement;
    [SerializeField] private float speed = 3;
    [SerializeField] private float prevRot = 0;
    [SerializeField] private bool loopTime = false;
    [SerializeField] private bool onLoop = false;
    [SerializeField] private bool offTheRamp = false;
    [SerializeField] private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()
    {
        float moveHorizontal = Input.GetAxis(Constants.Inputs.hori);
        float moveVertical = Input.GetAxis(Constants.Inputs.vert);
        movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (!loopTime)
        {
            Vector3 tempVect = transform.position + Camera.main.transform.TransformVector(movement);
            Quaternion rot;
            rot = transform.rotation;
            transform.LookAt(tempVect);
            transform.rotation = new Quaternion(rot.x, transform.rotation.y, 0, transform.rotation.w);
        }


        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.3f, -transform.up, out hit, 1.1f))
        {
            Quaternion quat = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
            //Debug.Log(hit.normal);
            transform.rotation = new Quaternion(quat.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            grounded = true;
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

        Vector3 rotate = new Vector3();

        if (loopTime && grounded)
        {
            rotate = new Vector3(0, prevRot, 0);

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
                speed = 18;
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
            Vector3 tempVect = Camera.main.transform.TransformVector(movement);
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
                //Debug.Log(Vector3.Dot(transform.up, Vector3.down));
                //if (Vector3.Dot(transform.up, Vector3.down) < 0.5f)
                //{
                //    Camera.main.transform.GetChild(0).rotation = new Quaternion(transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.GetChild(0).rotation.z, Camera.main.transform.GetChild(0).rotation.w);                 
                //}
                //else
                //{
                //    Camera.main.transform.GetChild(0).rotation = new Quaternion(-transform.rotation.x, Camera.main.transform.rotation.y, Camera.main.transform.GetChild(0).rotation.z, Camera.main.transform.GetChild(0).rotation.w);
                //}

                loopTime = true;
                rb.useGravity = false;
                //Vector3 tempVect = Camera.main.transform.GetChild(0).TransformVector(movement);
                Vector3 tempVect = transform.TransformVector(movement);
                tempVect *= speed * 0.5f;
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
}


























