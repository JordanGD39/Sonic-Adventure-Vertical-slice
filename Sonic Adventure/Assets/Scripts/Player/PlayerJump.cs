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
    [SerializeField] private bool targetAttack = false;

    [SerializeField] private Transform homingTarget = null;
    [SerializeField] private LayerMask enemyLayer;

    public bool Jumping { get { return jumping; } set { jumping = value; } }
    public bool Attacking { get { return attacking; } set { attacking = value; } }
    public bool HitHomingTarget { get { return hitHomingTarget; } set { hitHomingTarget = value; } }
    public bool TargetAttack { get { return targetAttack; } }

    private bool jumpPressed = false;
    private bool jumpHold = false;
    private bool bouncing = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMov = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Constants.Inputs.jump))
        {
            jumpPressed = true;
        }
        if (Input.GetButtonUp(Constants.Inputs.jump))
        {
            jumpPressed = false;
        }

        if (Input.GetButton(Constants.Inputs.jump))
        {
            jumpHold = true;
            extraJumpHeight += Input.GetAxis(Constants.Inputs.jump);
            if (extraJumpHeight > maxJumpHeight)
            {
                extraJumpHeight = maxJumpHeight;
            }
        }
        else
        {
            jumpHold = false;
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

        if (!Input.GetButton(Constants.Inputs.jump) && playerMov.Grounded || Input.GetButton(Constants.Inputs.jump) && playerMov.Grounded && extraJumpHeight >= maxJumpHeight)
        {
            extraJumpHeight = 1;
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);
            jumping = false;
        }
    }

    private void FixedUpdate()
    {
        if (jumpPressed && playerMov.Grounded)
        {
            extraJumpHeight = 1;
            rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
            transform.GetChild(1).gameObject.SetActive(true);
            transform.GetChild(0).gameObject.SetActive(false);
            jumping = true;
            AudioManager.instance.Play("Jump");
            jumpPressed = false;
        }
        else if (jumpPressed && !playerMov.Grounded && homingReady)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
            AudioManager.instance.Play("HomingAttack");
            if (homingReady)
            {
                StartCoroutine("HomingAttack");
            }            
        }

        if (jumpHold)
        {
            if (extraJumpHeight < maxJumpHeight)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Acceleration);
            }
        }

        if (bouncing)
        {
            rb.AddForce(transform.up * 500);
            bouncing = false;
        }
    }

    private IEnumerator HomingAttack()
    {
        attacking = true;
        hitHomingTarget = false;
        homingReady = false;
        jumpPressed = false;
        if (homingTarget != null)
        {
            while (!hitHomingTarget)
            {
                transform.LookAt(homingTarget);
                rb.velocity = transform.TransformVector(new Vector3(0, 0, homingSpeed));
                rb.useGravity = false;
                targetAttack = true;
                yield return new WaitForFixedUpdate();
            }                
        }
        else
        {                
            rb.AddForce(transform.forward * homingSpeed, ForceMode.Impulse);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            targetAttack = false;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForFixedUpdate();
            }          
        }
    }

    public void BounceOfEnemy()
    {
        rb.velocity = new Vector3(0, 0, 0);
        rb.useGravity = true;
        attacking = false;
        homingTarget = null;
        homingReady = true;
        bouncing = true;
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
