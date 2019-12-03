using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRelation : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;

    private RaycastHit hit;
    private bool wallHit;
    public bool WallHit { get { return wallHit; } set { wallHit = value; } }
    public RaycastHit Hit { get { return hit; } set { hit = value; } }
    public Transform PlayerTransform { get { return transform; }}

    void Start()
    {

    }

    void Update()
    {
        Vector3 posDifference = _camera.transform.position - transform.position;

        if (Physics.Raycast(transform.position, posDifference, out hit))
        {
            Debug.DrawRay(transform.position, posDifference, Color.blue);

            if (hit.collider.gameObject.tag == "MainCamera" || hit.collider.gameObject.tag == "Item")
            {
                wallHit = false;
            }
            else
            {
                wallHit = true;
            }
        }
    }
}
