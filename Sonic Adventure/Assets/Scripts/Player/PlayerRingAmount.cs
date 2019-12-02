using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRingAmount : MonoBehaviour
{
    const float SHOOTING_RADIUS = 200.0f;

    [SerializeField]
    private GameObject ring;

    [SerializeField]
    private Transform itemlist;

    private BoxCollider[] ringColliders;

    public bool hit;
    public int[] ringAmount = new int[2];

    private int count;

    void Start()
    {
        hit = false;
        count = 0;
        ringAmount[0] = 30;
    }

    private void Update()
    {
        CheckIfHit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            GetHit();
        }
    }

    private void GetHit() //A function which plays only once per collision
    {
        if (!hit)
        {
            //Only if the player is not already hit
            hit = true;

            if (ringAmount[0] >= 20)
            {
                ringAmount[0] = ShootRings(20);
            }
            else if (ringAmount[0] != 0)
            {
                ringAmount[0] = ShootRings(ringAmount[0]);
            }
            else
            {
                ringAmount[1] = 0;
            }
        }
    }

    private void CheckIfHit() //A function which plays constantly
    {
        if (hit)
        {
            count++;
            float step = count * Time.deltaTime;

            if (step >= 0.1f && step < 0.2f && ringAmount[1] > 0)
            {
                //Give those rings their Boxcollider back
                for (int i = 0; i < ringColliders.Length; i++)
                {
                    ringColliders[i].enabled = true;
                }
            }
            else if (step >= 1.5f)
            {
                //The point at which the player can pick up the rings again and get hit again
                hit = false;
            }
        }
        else
        {
            count = 0;
        }
    }

    private int ShootRings(int maxAmount)
    {
        BoxCollider[] thisRingCollider = new BoxCollider[maxAmount];

        float[] shootingAngle = new float[3];
        shootingAngle[0] = 0; //Angle in radians
        shootingAngle[1] = 1; //X-value based on angle
        shootingAngle[2] = 0; //Z-value based on angle

        for (int i = 0; i < maxAmount; i++)
        {
            shootingAngle[1] = Mathf.Cos(shootingAngle[0]);
            shootingAngle[2] = -Mathf.Sin(shootingAngle[0]);

            GameObject thisRing;
            thisRing = Instantiate(ring, transform.position, transform.rotation);
            thisRing.transform.parent = itemlist;

            thisRingCollider[i] = thisRing.GetComponent<BoxCollider>();
            thisRingCollider[i].enabled = false;

            Rigidbody ringBehaviour = thisRing.GetComponent<Rigidbody>();
            ringBehaviour.AddForce((itemlist.transform.up * 2 + new Vector3(shootingAngle[1], 0.0f, shootingAngle[2])) * SHOOTING_RADIUS);
            Destroy(thisRing, 8.0f);

            //Debug.Log("An angle of " + (shootingAngle[0]/ (2 * Mathf.PI) * 360) + " degrees. {x: " + shootingAngle[1] + ", z: " + shootingAngle[2] + "}");

            shootingAngle[0] += ((2 * Mathf.PI) / maxAmount);
        }

        ringColliders = thisRingCollider;
        ringAmount[1] = maxAmount;

        return 0;
    }
}
