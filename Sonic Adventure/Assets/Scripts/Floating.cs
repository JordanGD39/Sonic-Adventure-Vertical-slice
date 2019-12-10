using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour
{
    private Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transform.position.y < targetPos.y)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 1, 0);
        }
    }
}
