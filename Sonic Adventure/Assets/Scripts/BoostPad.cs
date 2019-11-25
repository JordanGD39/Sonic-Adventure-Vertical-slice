using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    [SerializeField] private float secondsOutOfControl;
    [SerializeField] private float horInput;
    [SerializeField] private float verInput;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tags.player))
        {
            PlayerMovement mov = other.transform.parent.GetComponent<PlayerMovement>();
            mov.Speed = 50;
            StartCoroutine(mov.Boost(secondsOutOfControl, horInput, verInput));
        }
    }
}
