using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerMovement playerMov;
    private LightSpeedDash lightSpeedDash;
    private ModelManage modelManage;
    private Spindash spindash;
    private TrailRenderer trailRenderer;

    [SerializeField] private float jumpHeight;
    [SerializeField] private float homingSpeed;
    [SerializeField] private float homingSpeedLoss = 2;
    [SerializeField] private float extraJumpHeight = 1; //How long you hold the button
    [SerializeField] private float maxJumpHeight = 2;
    [SerializeField] private bool jumping = false;
    [SerializeField] private bool homingReady = false;
    [SerializeField] private float homingRange = 5;
    [SerializeField] private float bounceForce = 10;
    [SerializeField] private float trailTime = 0.5f;
    [SerializeField] private bool attacking = false;
    [SerializeField] private bool hitHomingTarget = false;
    [SerializeField] private bool targetAttack = false;
    [SerializeField] private Transform jumpBall;
    private Vector3 homingVector;

    [SerializeField] private Transform homingTarget = null;
    [SerializeField] private LayerMask enemyLayer;

    public bool Jumping { get { return jumping; } set { jumping = value; } }
    public bool Attacking { get { return attacking; } set { attacking = value; } }
    public bool HitHomingTarget { get { return hitHomingTarget; } set { hitHomingTarget = value; } }
    public bool TargetAttack { get { return targetAttack; } }
    public float BounceForce { get { return bounceForce; } }
    public Vector3 HomingVector { get { return homingVector; } }

    private bool jumpPressed = false;
    private bool jumpHold = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMov = GetComponent<PlayerMovement>();
        lightSpeedDash = GetComponent<LightSpeedDash>();
        modelManage = GetComponent<ModelManage>();
        modelManage.ChangeToNormalModel();
        spindash = GetComponent<Spindash>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        trailRenderer.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (lightSpeedDash.Dashing || playerMov.Boosting)
        {
            attacking = false;
            return;
        }

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
            homingVector = Vector3.zero;
        }
        else
        {
            if (homingVector != Vector3.zero)
            {
                homingVector = Vector3.Lerp(homingVector, Vector3.zero, homingSpeedLoss * Time.deltaTime);
            }

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

        if (spindash.Spinning || spindash.InBall)
        {
            return;
        }

        if ((!jumpHold && playerMov.Grounded) || (jumpHold && playerMov.Grounded && extraJumpHeight >= maxJumpHeight))
        {
            extraJumpHeight = 1;
            modelManage.ChangeToNormalModel();
            jumping = false;
        }

        if (jumpBall.gameObject.activeSelf)
        {
            jumpBall.GetChild(0).Rotate(2000 * Time.deltaTime, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (lightSpeedDash.Dashing)
        {
            return;
        }

        if (jumpPressed && playerMov.Grounded)
        {
            extraJumpHeight = 1;
            jumping = true;
            rb.AddForce(transform.up * jumpHeight, ForceMode.VelocityChange);
            modelManage.ChangeToBallModel();
            StopCoroutine("BallBlink");
            StartBallBlink();
            AudioManager.instance.Play("Jump");
            jumpPressed = false;
            playerMov.Speed *= 0.8f;
        }
        else if (jumpPressed && !playerMov.Grounded && homingReady)
        {
            modelManage.ChangeToBallModel();
            AudioManager.instance.Play("HomingAttack");
            trailRenderer.emitting = true;
            Invoke(nameof(StopTrail), trailTime);
            if (homingReady)
            {
                StartCoroutine(BallBlink());
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
    }

    private void StopTrail()
    {
        trailRenderer.emitting = false;
    }

    private IEnumerator HomingAttack()
    {
        hitHomingTarget = false;
        homingReady = false;
        jumpPressed = false;
        if (homingTarget != null)
        {
            attacking = true;

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
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            playerMov.Speed = homingSpeed;
            homingVector = transform.GetChild(0).forward;
            targetAttack = false;
            for (int i = 0; i < 10; i++)
            {
                yield return new WaitForFixedUpdate();
            }
            modelManage.ChangeToNormalModel();
        }
    }

    public void BounceOfEnemy()
    {
        if (homingTarget != null)
        {
            rb.velocity = new Vector3(0, 0, 0);
            homingTarget = null;
            playerMov.Speed = 0;
        }

        playerMov.Bounced = true;
        homingVector = Vector3.zero;
        rb.useGravity = true;
        attacking = false;
        Invoke(nameof(ReadyHomingAttack), 0.1f);
        //rb.AddForce(transform.up * bounceSpeed * (playerMov.Grounded ? 1.5f : 1), ForceMode.VelocityChange);
    }

    private void ReadyHomingAttack()
    {
        homingReady = true;
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

    public void StartBallBlink()
    {
        StartCoroutine("BallBlink");
    }

    private IEnumerator BallBlink()
    {
        GameObject ball = jumpBall.GetChild(0).GetChild(0).gameObject;
        GameObject sonicBallPose = jumpBall.GetChild(0).GetChild(1).gameObject;

        while (jumping || spindash.InBall)
        {
            sonicBallPose.SetActive(false);
            ball.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            ball.SetActive(false);
            sonicBallPose.SetActive(true);
            yield return new WaitForSeconds(0.05f);
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position + transform.TransformVector(new Vector3(0, 0, 6)), homingRange);
    //}
}
