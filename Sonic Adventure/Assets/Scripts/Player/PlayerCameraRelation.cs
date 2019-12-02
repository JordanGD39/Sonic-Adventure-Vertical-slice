using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraRelation : MonoBehaviour
{
    [SerializeField]
    private GameObject _camera;

    private bool wallHit;
    public bool WallHit { get { return wallHit; } set { wallHit = value; } }

    void Start()
    {

    }

    void Update()
    {
        RaycastHit hit;
        Vector3 posDifference = _camera.transform.position - transform.position;

        if (Physics.Raycast(transform.position, posDifference, out hit))
        {
            Debug.DrawRay(transform.position, -hit.normal * hit.distance, Color.blue);

            if (hit.collider.gameObject.tag != "MainCamera")
            {
                Debug.Log("Wall between!");
                wallHit = true;
            }
            else
            {
                wallHit = false;
            }
        }
    }
}
