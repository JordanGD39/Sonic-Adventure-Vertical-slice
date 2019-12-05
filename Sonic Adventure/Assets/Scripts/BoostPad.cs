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
        if (other.gameObject.CompareTag(Constants.Tags.playerCol) && !other.transform.parent.GetComponent<PlayerMovement>().Boosting && other.transform.parent.GetComponent<PlayerJump>().enabled || other.gameObject.CompareTag(Constants.Tags.playerCol) && teleport)
        {
            PlayerMovement mov = other.transform.parent.GetComponent<PlayerMovement>();
            if (teleport)
            {                
                other.transform.parent.position = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
                mov.StopBoost();
            }
            playerRb = other.transform.parent.GetComponent<Rigidbody>();
            other.transform.parent.GetComponent<PlayerJump>().Attacking = false;
            other.transform.parent.GetComponent<PlayerJump>().enabled = false;            
            if (mov.Grounded)
            {                
                mov.Speed = speed;                
                mov.BoostPad(secondsOutOfControl, transform);
                if (addForce)
                {
                    //playerRb.AddForce(transform.forward * speed * 100);
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
        while (!mov.Grounded)
        {
            mov.Movement = new Vector3(0, 0, 0);
            playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
            yield return null;
        }
        
        mov.Speed = speed;
        mov.BoostPad(secondsOutOfControl, transform);
        if (other.GetComponent<SphereCollider>() != null)
        {
            other.transform.parent.GetChild(0).gameObject.SetActive(true);
            other.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (ramp)
        {
            playerRb.AddForce(transform.forward * speed * 85);
            ramp = false;
        }
    }
}
