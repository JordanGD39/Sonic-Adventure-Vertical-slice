using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    
    private Vector3 movement;
    [SerializeField] private float speed = 3;
    [SerializeField] private bool loopTime = false;
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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0, moveVertical);

        if (!loopTime)
        {
            Vector3 tempVect = transform.localPosition + Camera.main.transform.TransformVector(movement);
            Quaternion rot;
            rot = transform.localRotation;
            transform.LookAt(tempVect);
            transform.localRotation = new Quaternion(rot.x, transform.localRotation.y, 0, transform.localRotation.w);
        }
        else
        {
            Vector3 tempVect = transform.localPosition + transform.TransformVector(movement);
            Quaternion rot;
            rot = transform.localRotation;
            transform.LookAt(tempVect);
            transform.localRotation = new Quaternion(rot.x, transform.localRotation.y, 0, transform.localRotation.w);
        }

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.3f, -transform.up, out hit, 1.1f))
        {
            Quaternion quat = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
            Debug.Log(hit.normal);
            transform.localRotation = new Quaternion(quat.x, transform.localRotation.y, transform.localRotation.z, transform.localRotation.w);
            grounded = true;
        }
        else
        {
            grounded = false;
            transform.rotation = new Quaternion(0, transform.rotation.y, transform.rotation.z, transform.rotation.w);
            speed -= 0.6f;
            if (speed < 7)
            {
                speed += 0.6f;
            }
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
                loopTime = true;
                rb.useGravity = false;
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


























