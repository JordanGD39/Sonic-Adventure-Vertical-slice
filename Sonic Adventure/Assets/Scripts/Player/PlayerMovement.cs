using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    private Vector3 movement;
    [SerializeField] private float speed = 3;

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
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, -transform.up, out hit, 1.1f))
        {
            Quaternion quat = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
            Debug.Log(hit.normal);

            transform.rotation = new Quaternion(quat.x, transform.rotation.y, transform.rotation.z, quat.w);
        }
        //else
        //{
        //    transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
        //}
    }

    void FixedUpdate()
    {
        if (transform.rotation.x <= 0.6f && transform.rotation.x >= -0.6f)
        {
            rb.AddRelativeForce(movement * speed);
            rb.useGravity = true;
        }
        else
        {
            rb.AddRelativeForce(movement * (speed * 0.85f));
            rb.useGravity = false;
        }              
    }
}
