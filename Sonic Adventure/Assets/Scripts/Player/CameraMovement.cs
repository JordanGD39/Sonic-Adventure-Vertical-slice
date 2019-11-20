using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject _sonic;

    //Camera position
    private Transform transform;

    //Camera and player position difference
    private Vector3 difPos;
    
    void Start()
    {
        transform = GetComponent<Transform>();
        difPos = new Vector3(0.0f, 2.75f, -7.0f);
    }

    void Update()
    {
        transform.position = _sonic.transform.position + difPos;
    }
}
