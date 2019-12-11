using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    private GameObject ui;

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.playerCol))
        {
            while (!other.GetComponentInParent<PlayerMovement>().Grounded)
            {
                other.GetComponentInParent<PlayerMovement>().Movement = new Vector3(0, 0, 0);
                other.GetComponentInParent<Rigidbody>().velocity = new Vector3(0, other.GetComponentInParent<Rigidbody>().velocity.y, 0);
            }

            other.transform.GetComponentInChildren<Animator>().SetTrigger("Win");
            other.GetComponentInParent<PlayerMovement>().enabled = false;
            other.GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            Camera.main.GetComponent<AutoCamera>().Stop = true;
            Camera.main.GetComponent<ThirdPersonCameraControl>().Stop = true;
            other.transform.parent.LookAt(new Vector3(0, Camera.main.transform.position.y, 0));
        }
    }
}
