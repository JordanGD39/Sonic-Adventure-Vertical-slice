using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Physics
    private Rigidbody rb;
    private Vector3 inputVector;
    [SerializeField] private float speed = 3;

    private Vector3 upright;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }    

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = new Vector3(transform.right.x * (Input.GetAxis("Horizontal") * speed),rb.velocity.y, transform.forward.z * (Input.GetAxis("Vertical") * speed));
    }

    private void Update()
    {
        //transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));

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
}
