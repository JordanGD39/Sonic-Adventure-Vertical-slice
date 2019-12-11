using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Vector3 targetPos;

    private bool wave = false;
    private bool goingUp = false;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < targetPos.y && !goingUp || wave)
        {
            if (!wave)
            {
                GetComponent<Rigidbody>().velocity = Vector3.up;
            }
            else
            {
                GetComponent<Rigidbody>().velocity = -Vector3.up * 50;                
            }
        }
        else if (goingUp)
        {
            GetComponent<Rigidbody>().velocity = Vector3.up * 55;
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
