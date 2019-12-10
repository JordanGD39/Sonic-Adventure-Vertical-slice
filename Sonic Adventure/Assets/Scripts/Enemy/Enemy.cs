using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool hit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.playerCol))
        {
            if (other.GetComponent<SphereCollider>() != null)
            {
                PlayerJump playerJump = other.GetComponentInParent<PlayerJump>();

                playerJump.HitHomingTarget = true;

                playerJump.BounceOfEnemy();

                Destroy(gameObject, 0.1f);
            }
        }
    }
}
