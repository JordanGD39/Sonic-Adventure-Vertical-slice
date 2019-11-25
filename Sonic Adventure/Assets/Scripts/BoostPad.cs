using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

        }
    }
}
