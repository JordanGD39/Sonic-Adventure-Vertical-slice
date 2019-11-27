using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerMovement playerMov;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float extraJumpHeight = 1; //How long you hold the button
    [SerializeField] private float maxJumpHeight = 2; //How long you hold the button
    [SerializeField] private bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMov = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            extraJumpHeight += Input.GetAxis("Jump");
            if (extraJumpHeight > maxJumpHeight)
            {
                extraJumpHeight = maxJumpHeight;
            }
        }

        if (!Input.GetButton("Jump") && playerMov.Grounded || Input.GetButton("Jump") && playerMov.Grounded && extraJumpHeight >= maxJumpHeight)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && playerMov.Grounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
            extraJumpHeight = 1;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
        }

        if (Input.GetButton("Jump"))
        {
            if (extraJumpHeight < maxJumpHeight)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Acceleration);
            }
        }
    }
}
