using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour
{
    const float ROTATION_SPEED = 10.0f;

    private float thisRotation = 0.0f;

    private bool alreadyGivingPlayer = false;

    private Rigidbody rb;

    [SerializeField] private GameObject fx;

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
        if (other.gameObject.CompareTag(Constants.Tags.playerCol))
        {
            if (!alreadyGivingPlayer)
            {
                PlayerRingAmount playerRings = other.gameObject.transform.parent.GetComponent<PlayerRingAmount>();

                if (!playerRings.Hit)
                {
                    playerRings.RingAmount[0]++;
                    GameObject part = Instantiate(fx, transform.position, transform.rotation);
                    Destroy(part, 0.1f);
                    AudioManager.instance.Play("Ring");
                    Destroy(gameObject);
                    alreadyGivingPlayer = true;
                }
            }
        }
    }
}
