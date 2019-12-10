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
    public Transform PlayerTransform { get { return transform; } }

    void Update()
    {
        Vector3 posDifference = _camera.transform.position - transform.position;
        //int layermask = 1 << 9;

        if (Physics.Raycast(transform.position, posDifference, out hit))
        {
            Debug.DrawRay(transform.position, posDifference, Color.blue);

            if (hit.collider.gameObject.tag == Constants.Tags.mainCamera ||
                hit.collider.gameObject.tag == Constants.Tags.item ||
                hit.collider.gameObject.tag == Constants.Tags.trigger ||
                hit.collider.gameObject.layer == 9)
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
