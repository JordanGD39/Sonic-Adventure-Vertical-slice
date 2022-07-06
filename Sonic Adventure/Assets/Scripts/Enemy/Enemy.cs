using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool hit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hit)
        {
            return;
        }

        if (other.CompareTag(Constants.Tags.playerCol))
        {
            if (other.GetComponent<SphereCollider>() != null)
            {
                hit = true;

                PlayerJump playerJump = other.GetComponentInParent<PlayerJump>();

                playerJump.HitHomingTarget = true;

                playerJump.BounceOfEnemy();

                Destroy(gameObject, 0.1f);
            }
            else
            {
                PlayerRingAmount playerRing = other.GetComponentInParent<PlayerRingAmount>();

                playerRing.GetHit();
            }
        }
    }
}
