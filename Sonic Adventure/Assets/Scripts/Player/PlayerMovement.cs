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
    [SerializeField] private float wallSpeed = 3;
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
        movement = new Vector3(moveHorizontal * speed, rb.velocity.y, moveVertical * speed);
        movementForce = new Vector3(moveHorizontal, 0, moveVertical);

        Vector3 tempVect = transform.position + new Vector3(movement.x, 0, movement.z);
        transform.LookAt(new Vector3(tempVect.x, transform.position.y, tempVect.z));

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
            rb.velocity = Camera.main.transform.forward + movement;          
            loopTime = false;            
        }
        else
        {
            if (!loopTime)
            {
                wallSpeed = speed;
            }
            loopTime = true;
            if (transform.rotation.y > 0)
            {
                transform.rotation = new Quaternion(transform.rotation.x, 0, transform.rotation.z, transform.rotation.w);
            }
            else if (transform.rotation.y < 0)
            {
                transform.rotation = new Quaternion(transform.rotation.x, -180, transform.rotation.z, transform.rotation.w);
            }
            wallSpeed *= 0.995f;
            rb.AddRelativeForce(movementForce * wallSpeed);            
            rb.AddForce(Physics.gravity * 0.2f * rb.mass);
        }              
    }
}
