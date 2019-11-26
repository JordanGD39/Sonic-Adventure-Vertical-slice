using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Camera position
    [SerializeField]
    private Transform _camera;

    //Physics
    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 movementForce;

    [SerializeField] private float speed = 3;
    [SerializeField] private bool loopTime = false;
    [SerializeField] private bool offTheRamp = false;
    [SerializeField] private bool grounded = false;

    private Vector3 upright;
    private float playerPerspective;
    private float rotationSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerPerspective = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0, moveVertical);

        Vector3 tempVect = transform.localPosition + Camera.main.transform.TransformVector(movement);
        transform.LookAt(new Vector3(tempVect.x, transform.localPosition.y, tempVect.z));

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.45f, -transform.up, out hit, 1.1f))
        {
            Quaternion quat = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));

            transform.rotation = new Quaternion(quat.x, transform.rotation.y, transform.rotation.z, quat.w);

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
                //Debug.Log(tempVect);
                tempVect *= speed * 0.5f;
                //Debug.Log("after speed: " + tempVect);
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