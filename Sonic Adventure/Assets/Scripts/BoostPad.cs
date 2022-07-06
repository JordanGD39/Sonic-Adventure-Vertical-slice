using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField] private float secondsOutOfControl;
    [SerializeField] private float speed = 50;
    [SerializeField] private bool teleport = true;
    [SerializeField] private bool addForce = false;
    private bool ramp = false;

    private Rigidbody playerRb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tags.playerCol) && !other.GetComponentInParent<PlayerMovement>().Boosting && other.GetComponentInParent<PlayerJump>().enabled || other.gameObject.CompareTag(Constants.Tags.playerCol) && teleport)
        {
            PlayerMovement mov = other.GetComponentInParent<PlayerMovement>();

            if (teleport)
            {
                mov.transform.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
                mov.StopBoost();
            }

            playerRb = other.GetComponentInParent<Rigidbody>();

            if (mov.Grounded)
            {                
                mov.Speed = speed;                
                mov.BoostPad(secondsOutOfControl, transform);
                AudioManager.instance.Play("BoostPad");
                if (addForce)
                {                    
                    ramp = true;
                }
                if (other.GetComponent<SphereCollider>() != null)
                {
                    other.transform.parent.GetChild(0).gameObject.SetActive(true);
                    other.gameObject.SetActive(false);
                }
            }
            else
            {
                StartCoroutine(WaitUntilPlayerIsGrounded(mov, playerRb, other.gameObject));
            }
        }
    }

    private IEnumerator WaitUntilPlayerIsGrounded(PlayerMovement mov, Rigidbody playerRb, GameObject other)
    {
        if (!mov.Grounded)
        {
            mov.Movement = new Vector3(0, 0, 0);
            playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
            mov.LockedFall = true;
        }

        SphereCollider sphereCol = other.GetComponent<SphereCollider>();

        while (!mov.Grounded)
        {
            if (sphereCol != null)
            {
                other.transform.GetChild(0).Rotate(20, 0, 0);
            }
            yield return null;
        }

        mov.LockedFall = false;

        mov.Speed = speed;
        mov.BoostPad(secondsOutOfControl, transform);
        AudioManager.instance.Play("BoostPad");
        if (sphereCol != null)
        {
            other.transform.parent.GetChild(0).gameObject.SetActive(true);
            other.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (ramp)
        {
            playerRb.AddForce(transform.forward * speed, ForceMode.Impulse);
            ramp = false;
        }
    }
}
