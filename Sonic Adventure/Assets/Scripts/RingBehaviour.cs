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
        }
        else if (colliders[colliders.Length - 1].gameObject.tag == PLAYER_TAG)
        {
            PlayerRingAmount playerRings = colliders[colliders.Length - 1].transform.parent.GetComponent<PlayerRingAmount>();

            if (!playerRings.hit)
            {
                Destroy(gameObject);
            }
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PLAYER_TAG))
        {
            PlayerRingAmount playerRings = other.gameObject.transform.parent.GetComponent<PlayerRingAmount>();

            if (!playerRings.hit)
            {
                playerRings.ringAmount++;
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(PLAYER_TAG))
        {
            Collider playerCollider = collision.gameObject.GetComponentInChildren<Collider>();
            Physics.IgnoreCollision(playerCollider, GetComponent<BoxCollider>(), true);
        }
    }
}
