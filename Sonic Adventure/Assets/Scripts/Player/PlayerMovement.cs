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
        inputVector = new Vector3(Input.GetAxis("Horizontal") * speed, rb.velocity.y, Input.GetAxis("Vertical") * speed);
        rb.velocity = inputVector;
    }

    private void Update()
    {
        transform.LookAt(transform.position + new Vector3(inputVector.x, 0, inputVector.z));

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.5f, -(transform.up), out hit, 1.1f))
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Cross(transform.right, hit.normal));
        }
    }
}
