using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField] private float secondsOutOfControl;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tags.player))
        {
            other.transform.parent.position = transform.position;
            PlayerMovement mov = other.transform.parent.GetComponent<PlayerMovement>();
            mov.Speed = 50;
            StartCoroutine(mov.Boost(secondsOutOfControl, transform));
        }
    }
}
