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

    [SerializeField] private Vector2 flySpeed;

    private bool hit;
    private int[] ringAmount = new int[2];

    public int[] RingAmount { get { return ringAmount; } set { ringAmount = value; } }
    public bool Hit { get { return hit; } set { hit = value; } }

    private int count;
    private bool invincible = false;

    void Start()
    {
        hit = false;
        count = 0;
        ringAmount[0] = 0;        
        StartCoroutine(GameManager.instance.RingBlink());
    }

    private void Update()
    {
        CheckIfHit();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag(Constants.Tags.hazard))
        {
            GetHit();
        }
    }

    public void GetHit() //A function which plays only once per collision
    {
        if (!hit && !invincible)
        {
            //Only if the player is not already hit
            hit = true;
            invincible = true;

            if (ringAmount[0] >= 20)
            {
                ringAmount[0] = ShootRings(20);
            }
            else if (ringAmount[0] != 0)
            {
                ringAmount[0] = ShootRings(ringAmount[0]);
                StartCoroutine(GameManager.instance.RingBlink());
            }
            else
            {
                if (!GameManager.instance.Dying)
                {
                    ringAmount[1] = 0;
                    StartCoroutine("Damage");
                    StartCoroutine(GetComponent<PlayerDeath>().Die());
                }                
            }
        }
    }

    private void CheckIfHit() //A function which plays constantly
    {
        if (hit)
        {
            count++;
        }
        else
        {
            count = 0;
        }
    }

    private int ShootRings(int maxAmount)
    {
        BoxCollider[] thisRingCollider = new BoxCollider[maxAmount];

        StartCoroutine("Damage");

        AudioManager.instance.Play("LosingRings");

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
            thisRing.GetComponent<RingBehaviour>().droppedItem = true;

            Rigidbody ringBehaviour = thisRing.GetComponent<Rigidbody>();
            ringBehaviour.AddForce((itemlist.transform.up * 2 + new Vector3(shootingAngle[1], 0.0f, shootingAngle[2])) * SHOOTING_RADIUS);
            Destroy(thisRing, Constants.Value.ringSeconds);

            shootingAngle[0] += ((2 * Mathf.PI) / maxAmount);
        }

        ringColliders = thisRingCollider;
        ringAmount[1] = maxAmount;

        
        return 0;
    }

    private IEnumerator Damage()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        GetComponent<PlayerMovement>().Boosting = true;
        GetComponent<PlayerMovement>().Speed = 0;
        GetComponent<PlayerJump>().enabled = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().AddForce(transform.up * flySpeed.y, ForceMode.Impulse);
        GetComponent<Rigidbody>().AddForce(-transform.forward * flySpeed.x, ForceMode.Impulse);

        yield return new WaitForSeconds(1);

        while (!GetComponent<PlayerMovement>().Grounded)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1);

        GetComponent<PlayerMovement>().Boosting = false;
        hit = false;
        GetComponent<PlayerJump>().enabled = true;

        yield return new WaitForSeconds(1);

        invincible = false;        
    }
}
