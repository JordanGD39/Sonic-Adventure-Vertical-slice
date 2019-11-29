using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerMovement playerMov;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float homingSpeed;
    [SerializeField] private float extraJumpHeight = 1; //How long you hold the button
    [SerializeField] private float maxJumpHeight = 2;
    [SerializeField] private bool jumping = false;
    [SerializeField] private bool homingReady = false;
    [SerializeField] private float homingRange = 5;
    [SerializeField] private bool attacking = false;
    [SerializeField] private bool hitHomingTarget = false;

    [SerializeField] private Transform homingTarget = null;
    [SerializeField] private LayerMask enemyLayer;

    public bool Jumping { get { return jumping; } set { jumping = value; } }
    public bool Attacking { get { return attacking; } }
    public bool HitHomingTarget { get { return hitHomingTarget; } set { hitHomingTarget = value; } }
    public Transform HomingTarget { get { return homingTarget; } }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMov = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            extraJumpHeight += Input.GetAxis("Jump");
            if (extraJumpHeight > maxJumpHeight)
            {
                extraJumpHeight = maxJumpHeight;
            }
        }

        if (playerMov.Grounded)
        {
            homingReady = true;
            homingTarget = null;
            attacking = false;
        }
        else
        {
            Collider[] enemyCol = Physics.OverlapSphere(transform.position + transform.TransformVector(new Vector3(0, 0, 6)), homingRange, enemyLayer);
            if (enemyCol.Length != 0)
            {
                homingTarget = GetClosestEnemy(enemyCol);
            }

            if (homingTarget != null)
            {
                CheckEnemyStillInRange(enemyCol);
            }
        }

        if (!Input.GetButton("Jump") && playerMov.Grounded || Input.GetButton("Jump") && playerMov.Grounded && extraJumpHeight >= maxJumpHeight)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            jumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && playerMov.Grounded)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
            extraJumpHeight = 1;
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            jumping = true;
        }
        else if (Input.GetButtonDown("Jump") && !playerMov.Grounded && homingReady)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine("HomingAttack");    
        }

        if (Input.GetButton("Jump"))
        {
            if (extraJumpHeight < maxJumpHeight)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Acceleration);
            }
        }        
    }

    private IEnumerator HomingAttack()
    {
        if (homingReady)
        {
            attacking = true;
            hitHomingTarget = false;
            homingReady = false;
            if (homingTarget != null)
            {
                while (!hitHomingTarget)
                {
                    transform.LookAt(homingTarget);
                    rb.velocity = transform.TransformVector(new Vector3(0, 0, homingSpeed));
                    rb.useGravity = false;
                    yield return null;
                }
                rb.velocity = new Vector3(0, 0, 0);
                rb.useGravity = true;
                attacking = false;
                homingTarget = null;
                homingReady = true;
                rb.AddForce(transform.up * 500);
            }
            else
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(transform.forward * homingSpeed * 75);                
                rb.useGravity = false;
                yield return new WaitForSeconds(0.1f);
                rb.useGravity = true;                
            }
        }
    }

    private Transform GetClosestEnemy(Collider[] enemies)
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Collider t in enemies)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin;
    }

    private void CheckEnemyStillInRange(Collider[] enemyCol)
    {
        if (!attacking)
        {
            bool inRange = false;

            for (int i = 0; i < enemyCol.Length; i++)
            {
                if (enemyCol[i] == homingTarget.GetComponent<BoxCollider>())
                {
                    inRange = true;
                }
            }

            if (!inRange)
            {
                homingTarget = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + transform.TransformVector(new Vector3(0, 0, 6)), homingRange);
    }
}
