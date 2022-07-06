using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Vector3 targetPos;

    private bool wave = false;
    private bool goingUp = false;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < targetPos.y && !goingUp || wave)
        {
            if (!wave)
            {
                rb.velocity = Vector3.up;
            }
            else
            {
                if (transform.position.y > targetPos.y - 4)
                {
                    rb.velocity = -Vector3.up * 50;
                }                              
            }
        }
        else if (goingUp)
        {
            if (transform.position.y < targetPos.y + 4)
            {
                rb.velocity = Vector3.up * 55;
            }            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.wave))
        {
            StopCoroutine("WaveDetected");
            StartCoroutine("WaveDetected");
        }
    }

    private IEnumerator WaveDetected()
    {
        wave = true;
        yield return new WaitForSeconds(0.15f);
        wave = false;
        goingUp = true;
        yield return new WaitForSeconds(0.11677f);
        goingUp = false;
    }
}
