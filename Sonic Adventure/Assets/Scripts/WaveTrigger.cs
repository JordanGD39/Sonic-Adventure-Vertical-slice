using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    [SerializeField] private GameObject wave;
    [SerializeField] private Transform spawn;

    public void ExpandWave()
    {
        Instantiate(wave, spawn.position, wave.transform.rotation);
    }
}
