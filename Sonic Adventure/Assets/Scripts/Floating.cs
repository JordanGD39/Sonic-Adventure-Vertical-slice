using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Vector3 targetPos;

    private bool wave = false;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < targetPos.y || wave)
        {
            GetComponent<Rigidbody>().velocity = Vector3.up;
        }
        else if (transform.position.y > targetPos.y && !wave)
        {
            GetComponent<Rigidbody>().velocity = -Vector3.up;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.wave))
        {
            StartCoroutine("WaveDetected");
        }
    }

    private IEnumerator WaveDetected()
    {
        wave = true;
        yield return new WaitForSeconds(1);
        wave = false;
    }
}
