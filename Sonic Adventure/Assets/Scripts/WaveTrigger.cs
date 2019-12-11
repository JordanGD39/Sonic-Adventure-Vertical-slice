using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTrigger : MonoBehaviour
{
    [SerializeField] private GameObject wave;
    [SerializeField] private Transform spawn;

    private bool done = false;

    public void ExpandWave()
    {
        Instantiate(wave, spawn.position, wave.transform.rotation);
        if (!done)
        {
            StartCoroutine("ChangePos");
        }
    }

    private IEnumerator ChangePos()
    {
        done = true;
        yield return new WaitForSeconds(5);
        transform.parent.position = new Vector3(285.2f, 0, 99.1f);
        transform.parent.rotation = Quaternion.Euler(0, 158.6f, 0);        
        GetComponent<Animator>().SetTrigger("Player Arrival");        
    }
}
