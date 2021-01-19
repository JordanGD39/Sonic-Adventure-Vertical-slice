using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour
{
    const float ROTATION_SPEED = 625;

    private float thisRotation = 0.0f;

    private bool alreadyGivingPlayer = false;
    public bool droppedItem;

    private int[] count = new int[2];

    private Rigidbody rb;
    private MeshRenderer mRend;

    [SerializeField] private GameObject fx;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mRend = GetComponent<MeshRenderer>();

        count[0] = 0;
        count[1] = 0;
    }

    private void Update()
    {
        thisRotation += ROTATION_SPEED * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0.0f, thisRotation, 0.0f);

        if (droppedItem)
        {
            count[0]++;
        }
        else
        {
            count[0] = 0;
        }

        SetMeshVisibility(count[0], count[1]);
    }

    private void SetMeshVisibility(int coun, int t)
    {
        float step = coun * Time.deltaTime;
        float flip = t * Time.deltaTime;

        if (step >= (Constants.Value.ringSeconds - 1.0f))
        {
            count[1]++;
        }

        if (flip >= 0.07f && flip < 0.14f)
        {
            mRend.enabled = false;
        }
        else if (flip >= 0.14f && flip < 0.21f)
        {
            mRend.enabled = true;
        }
        else if(flip >= 0.21f)
        {
            count[1] = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(Constants.Tags.playerCol))
        {
            if (!alreadyGivingPlayer)
            {
                PlayerRingAmount playerRings = other.gameObject.transform.parent.GetComponent<PlayerRingAmount>();

                if (!playerRings.Hit)
                {
                    playerRings.RingAmount[0]++;
                    GameObject part = Instantiate(fx, transform.position, transform.rotation);
                    Destroy(part, 0.1f);
                    AudioManager.instance.Play("Ring");
                    Destroy(gameObject);
                    alreadyGivingPlayer = true;
                }
            }
        }
    }
}
