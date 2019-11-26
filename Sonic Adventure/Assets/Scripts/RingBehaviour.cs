using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour
{
    const string PLAYER_TAG = "Player";
    const float ROTATION_SPEED = 10.0f;

    private float thisRotation = 0.0f;

    private void Start()
    {

    }

    private void Update()
    {
        thisRotation += ROTATION_SPEED;
        transform.rotation = Quaternion.Euler(0.0f, thisRotation, 0.0f);
    }

    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.y);

        if (colliders.Length <= 0)
        {
            transform.position -= (new Vector3(0.0f, 3.0f, 0.0f) * Time.deltaTime);
        }
        else if (colliders[colliders.Length - 1].gameObject.tag == PLAYER_TAG)
        {
            Destroy(this.gameObject);
        }
    }
}
