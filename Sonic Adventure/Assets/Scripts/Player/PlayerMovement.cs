using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 movementForce;
    [SerializeField] private float speed = 3;
    private bool loopTime = false;

    private Vector3 upright;

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
        movement = new Vector3(Camera.main.transform.right.x * (moveHorizontal * speed), 0, Camera.main.transform.forward.z * (moveVertical * speed));
        movementForce = new Vector3(moveHorizontal, 0, moveVertical);

        if (!loopTime)
        {
            transform.LookAt(transform.position + new Vector3(movement.x, 0, movement.z));
        }

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out hit, 1.1f))
        {
            Quaternion quat = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));            

            transform.rotation = new Quaternion(quat.x, transform.rotation.y, transform.rotation.z, quat.w);
        }
        else
        {
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

        if (transform.rotation.x <= 0.3f && transform.rotation.x >= -0.3f)
        {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            rb.useGravity = true;
            loopTime = false;
        }
        else
        {
            loopTime = true;
            rb.AddRelativeForce(movementForce * (speed * 0.75f));
            rb.useGravity = false;
        }              
    }
}
