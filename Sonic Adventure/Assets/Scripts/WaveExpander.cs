using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveExpander : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5);  
    }
    
    // Update is called once per frame
    void Update()
    {
        GetComponent<SphereCollider>().radius += 0.5f;
    }
}
