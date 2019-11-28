using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRingAmount : MonoBehaviour
{
    [SerializeField]
    private GameObject ring;

    [SerializeField]
    private Transform itemlist;

    public bool hit;
    public int ringAmount;

    private int count;

    void Start()
    {
        hit = false;
        count = 0;
        ringAmount = 30;
    }

    private void Update()
    {
        CheckIfHit();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            //Debug.Log("Ouwie! OwO");
            GetHit();
        }
    }

    private void GetHit() //A function which plays only once
    {
        if (!hit)
        {
            //Only if the player is not already hit
            hit = true;

            if (ringAmount >= 20)
            {
                ringAmount = ShootRings(20);
            }
            else if (ringAmount != 0)
            {
                ringAmount = ShootRings(ringAmount);
            }
        }
    }

    private void CheckIfHit() //A function which plays constantly
    {
        if (hit)
        {
            count++;
            float step = count * Time.deltaTime;

            if (step >= 1.5f)
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
        for (int i = 0; i < maxAmount; i++)
        {
            GameObject thisRing;
            thisRing = Instantiate(ring, transform.position, transform.rotation);
            thisRing.transform.parent = itemlist;

            Rigidbody ringBehaviour = thisRing.GetComponent<Rigidbody>();
            ringBehaviour.AddRelativeForce(transform.up * 400.0f);
            Destroy(thisRing, 8.0f);
        }

        return 0;
    }
}
