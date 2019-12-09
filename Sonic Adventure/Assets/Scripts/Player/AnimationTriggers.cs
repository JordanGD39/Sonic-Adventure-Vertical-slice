using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _triggers;

    [SerializeField]
    private Animator _orcaAnimator;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == _triggers[0].name)
        {
            //Debug.Log("Triggered");
            _orcaAnimator.SetTrigger("Player Arrival");
        }
    }
}
