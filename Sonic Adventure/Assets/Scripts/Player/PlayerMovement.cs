using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    private float horVel;
    private float verVel;
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
        horVel = Input.GetAxis("Horizontal");
        verVel = Input.GetAxis("Vertical");

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, -(transform.up), out hit, 1.1f))
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.y, transform.rotation.z);
        }
    }

    void FixedUpdate()
    {
        rb.AddForce(new Vector3(transform.right.x * (horVel * speed),rb.velocity.y, transform.forward.z * (verVel * speed)));
    }
}
