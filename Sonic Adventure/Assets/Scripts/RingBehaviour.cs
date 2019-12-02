using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour
{
    const string PLAYER_TAG = "Player";
    const float ROTATION_SPEED = 10.0f;

    private float thisRotation = 0.0f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        thisRotation += ROTATION_SPEED;
        transform.rotation = Quaternion.Euler(0.0f, thisRotation, 0.0f);

        /*Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.y);

        if (colliders.Length <= 0)
        {
            transform.position -= (new Vector3(0.0f, 3.0f, 0.0f) * Time.deltaTime);
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            PlayerRingAmount playerRings = other.gameObject.transform.parent.GetComponent<PlayerRingAmount>();

            if (!playerRings.hit)
            {
                playerRings.ringAmount[0]++;
                Destroy(gameObject);
            }
        }
    }
}
