using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Camera position
    [SerializeField]
    private Transform _camera;

    private Rigidbody rb;
    private PlayerJump playerJump;
    private PlayerRingAmount playerRing;
    private Spindash spindash;
    private LightSpeedDash lightSpeedDash;

    private Vector3 movement;
    private Vector3 smoothMovement;
    private Vector3 camConvertedMovement;
    private Vector3 calculatedNormalMovement;

    [SerializeField] private Transform forward;
    [SerializeField] private Transform potentialForward;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float smoothSpeed = 10;
    [SerializeField] private float speed = 3;
    [SerializeField] private float walkingSpeed = 15;
    [SerializeField] private float reachTopSpeedSpeed = 20;
    [SerializeField] private float accelerationSpeed = 31;
    [SerializeField] private float topSpeedAccelerationSpeed = 12;
    [SerializeField] private float deccelerationSpeed = 31;
    [SerializeField] private float brakingMultipler = 2;
    [SerializeField] private float overMaxSpeedSpeedLoss = 16;
    [SerializeField] private float turningSpeedLoss = 32;
    [SerializeField] private float boostSpeedLoss = 8;
    [SerializeField] private float maxSpeed = 35;
    [SerializeField] private float absoluteMaxSpeed = 100;
    [SerializeField] private float slopeInfluence = 17;
    [SerializeField] private float slopeThreshold = 40;
    [SerializeField] private float steepSlopeThreshold = 50;
    [SerializeField] private float currentSlopeThreshold = 40;
    [SerializeField] private float currentSteepSlopeThreshold = 50;
    [SerializeField] private float minWallRunSpeed = 20;
    [SerializeField] private float maxAirSpeed = 10;
    [SerializeField] private float airSpeedLoss = 6;
    [SerializeField] private float airSpeedGain = 12;
    [SerializeField] private float normalSmoothing = 6.5f;
    [SerializeField] private float extraPercentAfterLeavingGround = 0.2f;
    [SerializeField] private float maxAnimSpeed = 30;
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool boosting = false;
    [SerializeField] private bool clingToGround = false;
    [SerializeField] private bool braking = false;
    [SerializeField] private bool lowSpeedFall = false;
    [SerializeField] private bool rollingDownLowSpeed = false;
    [SerializeField] private float turningThreshold = 0.8f;
    [SerializeField] private bool turning = false;
    private float hitAngle = 0;

    private float boostSec = 0;
    private Transform boostTransform = null;
    private Animator anim;

    public float Speed { get { return speed; }  set { speed = value; } }
    public float MaxSpeed { get { return maxSpeed; } }
    public bool Boosting { get { return boosting; }  set { boosting = value; } }
    public bool Grounded { get { return grounded; } }
    public bool LowSpeedFall { get { return lowSpeedFall; } }
    public float HitAngle { get { return hitAngle; } }
    public bool LockedFall { get; set; } = false;
    public Vector3 Movement { get { return movement; } set { movement = value; } }
    public bool Bounced { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerJump = GetComponent<PlayerJump>();
        playerRing = GetComponent<PlayerRingAmount>();
        lightSpeedDash = GetComponent<LightSpeedDash>();
        spindash = GetComponent<Spindash>();
        anim = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
    }

    private void Update()
    {
        if (!rollingDownLowSpeed)
        {
            MovementChoose();
        }

        if (rollingDownLowSpeed && speed > 20 || !spindash.InBall)
        {
            rollingDownLowSpeed = false;
        }
        
        currentSlopeThreshold = !spindash.InBall ? slopeThreshold : spindash.SlopeThreshold;
        currentSteepSlopeThreshold = !spindash.InBall ? steepSlopeThreshold : spindash.SlopeThreshold;

        LookAtDirection(boosting ? forward : potentialForward, camConvertedMovement);

        GroundAlignment();
        anim.SetBool("Grounded", grounded);

        if (lightSpeedDash.Dashing)
        {
            return;
        }

        if (!boosting)
        {
            BrakeCheck();
        }

        if (!spindash.SpindashStarted && !boosting)
        {
            LookAtDirection(forward, camConvertedMovement);
        }

        if (grounded && !playerRing.Hit && !spindash.Spinning)
        {
            Acceleration();
        }

        float animSpeed = speed / maxAnimSpeed;

        anim.SetFloat("Speed", animSpeed);
        anim.SetBool("Braking", braking);
        anim.SetBool("Hit", playerRing.Hit);
    }

    private void GroundAlignment()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, 0.3f, -transform.up, out hit, 1.1f, layerMask) && (!lowSpeedFall || lowSpeedFall && spindash.InBall && hitAngle < 95) && !Bounced)
        {
            if (!hit.collider.isTrigger)
            {
                float newHitAngle = Vector3.Angle(Vector3.up, hit.normal);

                if (hit.normal != Vector3.up && Mathf.Abs(newHitAngle - hitAngle) > 85)
                {
                    return;
                }

                transform.up -= (transform.up - hit.normal) * normalSmoothing * Time.deltaTime;
                //Debug.Log(transform.rotation);
                hitAngle = newHitAngle;

                if (!grounded)
                {
                    float normalCalc = -(hit.normal.y - 1);

                    if (normalCalc > 0.1f)
                    {
                        normalCalc += 0.2f;
                    }

                    if (normalCalc > 1)
                    {
                        normalCalc = 1;
                    }

                    float tempSpeed = rb.velocity.magnitude * normalCalc;

                    if (tempSpeed > 3)
                    {
                        camConvertedMovement = rb.velocity;
                        LookAtDirection(forward, camConvertedMovement);
                    }
                }

                grounded = true;
                clingToGround = !boosting;
                
                calculatedNormalMovement = hit.normal != Vector3.up && !boosting ? Vector3.ProjectOnPlane(camConvertedMovement, hit.normal) : camConvertedMovement;
            }

            if (hit.collider.gameObject.CompareTag(Constants.Tags.boostPad))
            {
                grounded = true;
            }
        }
        else
        {
            if (grounded && !boosting)
            {                
                speed = Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z);
            }

            calculatedNormalMovement = camConvertedMovement;

            hitAngle = 0;
            grounded = false;
            transform.rotation = new Quaternion(0, 0, 0, transform.rotation.w);
            lowSpeedFall = false;
            rollingDownLowSpeed = false;

            if (braking)
            {
                speed -= deccelerationSpeed * 1.5f * Time.deltaTime;
            }

            if (speed > maxAirSpeed || movement == Vector3.zero)
            {
                speed -= airSpeedLoss * Time.deltaTime;
            }        
            else if(!playerRing.Hit && !braking)
            {
                speed += airSpeedGain * Time.deltaTime;
            }

            if (speed < 0)
            {
                speed = 0;
            }
        }
    }

    private void MovementChoose()
    {
        if (playerRing.Hit || GameManager.instance.Dying)
        {
            return;
        }

        if (!boosting)
        {
            float h = Input.GetAxis(Constants.Inputs.hori);
            float v = Input.GetAxis(Constants.Inputs.vert);

            movement = new Vector3(h, 0, v);
            movement.Normalize();

            smoothMovement = Vector3.Lerp(smoothMovement, movement, smoothSpeed * Time.deltaTime);

            turning = Vector3.Dot(smoothMovement, movement) < turningThreshold;            

            camConvertedMovement = _camera.TransformVector(smoothMovement);
        }
        else
        {
            braking = false;
            movement = Vector3.forward;

            camConvertedMovement = forward.forward;
        }
    }

    private void BrakeCheck()
    {
        float dot = Vector3.Dot(rb.velocity.normalized, potentialForward.forward);

        if ((dot < -0.4f && !braking || dot <= 0 && braking) && speed > 1)
        {
            braking = true;
            camConvertedMovement = rb.velocity.normalized;
            calculatedNormalMovement = camConvertedMovement;
        }
        else
        {
            braking = false;
        }
    }

    private void LookAtDirection(Transform target, Vector3 lookDirection)
    {
        Vector3 tempVect = transform.position + lookDirection;
        //tempVect.y = transform.position.y;
        target.LookAt(tempVect);
        target.localRotation = new Quaternion(0, target.localRotation.y, 0, target.localRotation.w);
    }

    private void FixedUpdate()
    {
        if (playerJump.Attacking || LockedFall || playerRing.Hit || lightSpeedDash.Dashing)
        {
            return;
        }

        Vector3 velocity = calculatedNormalMovement;

        if (!boosting)
        {
            velocity += forward.forward;
        }

        velocity.Normalize();

        if (!grounded)
        {
            velocity += playerJump.HomingVector;
            velocity.Normalize();
        }

        velocity *= speed;    

        if (hitAngle < currentSlopeThreshold || playerJump.Jumping || speed < minWallRunSpeed)
        {
            if (hitAngle > currentSteepSlopeThreshold && speed < minWallRunSpeed)
            {
                lowSpeedFall = true;

                if (spindash.InBall && speed < 1)
                {
                    rollingDownLowSpeed = true;
                    LookAtDirection(forward, -rb.velocity);
                    speed = 1;
                    velocity = rb.velocity;
                    lowSpeedFall = false;
                }
            }

            if (!(lowSpeedFall && spindash.InBall) || playerJump.Jumping && hitAngle < 90)
            {
                velocity.y = rb.velocity.y;
            }
        }
        
        rb.velocity = velocity;

        if (clingToGround && !playerJump.Jumping && grounded && !Bounced)
        {
            rb.AddForce(-transform.up * 100 * (speed / (maxSpeed / 2)));
        }

        if (Bounced)
        {
            rb.AddForce(transform.up * playerJump.BounceForce, ForceMode.VelocityChange);
            Bounced = false;
        }
    }

    private float GetNormalAnglePercent(bool overload)
    {
        float percent = hitAngle / 90;

        if (!overload && percent > 1)
        {
            percent = 1;
        }

        return percent;
    }

    private void Acceleration()
    {
        bool goingUp = hitAngle > currentSlopeThreshold && rb.velocity.y >= 0;

        if (goingUp)
        {
            float forwardAngle = forward.forward.y * 1.7f;
            float clampDriveUpSpeed = Mathf.Clamp(forwardAngle, 0, 1);

            float rollMultiplier = spindash.InBall ? spindash.SlopeMultiplier : 1;

            float speedLoss = slopeInfluence * spindash.SlopeMultiplier;

            speed -= speedLoss * GetNormalAnglePercent(true) * clampDriveUpSpeed * Time.deltaTime;
        }

        bool goingDown = hitAngle > currentSlopeThreshold && rb.velocity.y < 0;

        Vector3 mov = movement.normalized;
        float acc = Mathf.Abs(mov.x) + Mathf.Abs(mov.z);
        
        if (acc > 1)
        {
            acc = 1;
        }

        if ((speed <= maxSpeed || goingDown) && speed < absoluteMaxSpeed)
        {
            float accSpeed = speed <= reachTopSpeedSpeed && !goingUp ? accelerationSpeed * Time.deltaTime : topSpeedAccelerationSpeed * Time.deltaTime;

            if ((acc > 0.1f && !spindash.InBall || spindash.InBall && goingDown || !spindash.InBall && goingDown) && !braking)
            {
                if (turning && !spindash.InBall && !goingDown)
                {
                    if (speed > walkingSpeed)
                    {
                        Deccelerate(turningSpeedLoss, 0);
                    }                    
                }

                float forwardAngle = -forward.forward.y * 1.7f;
                float clampDriveDownSpeed = goingDown ? Mathf.Clamp(forwardAngle, 0, 1) : 1;

                float rollMultiplier = spindash.InBall ? spindash.SlopeMultiplier : 1;

                float slopeSpeed = goingDown ? slopeInfluence * rollMultiplier : 0;
                if (goingDown && (acc == 0 && !spindash.InBall || spindash.InBall))
                {
                    acc = spindash.InBall ? 1 : 0.2f;
                }
                slopeSpeed *= GetNormalAnglePercent(true);
                slopeSpeed *= Time.deltaTime;

                speed += (accSpeed + slopeSpeed) * acc * clampDriveDownSpeed;
            }
            else
            {
                if (speed > 0.1f)
                {
                    Deccelerate(0, 0);
                }
                else if (speed < -0.1f)
                {
                    speed += deccelerationSpeed * Time.deltaTime;
                }
                else
                {
                    speed = 0;
                }
            }
        }
        else
        {
            if (boosting)
            {
                speed -= boostSpeedLoss * Time.deltaTime;
            }
            else
            {
                Deccelerate(overMaxSpeedSpeedLoss, acc);
            }
        }
    }

    private void Deccelerate(float extraSpeedLoss, float input)
    {
        float brakeMultiplier = braking ? brakingMultipler : 1;

        if (rb.velocity.y < 0)
        {
            brakeMultiplier *= Mathf.Abs(GetNormalAnglePercent(false) - 1);
        }

        float rollMultiplier = spindash.InBall ? spindash.DeccMultiplier : 1;
        float i = !braking ? Mathf.Abs(input - 1) : 1;

        if (i < 0.5f)
        {
            i = 0.5f;
        }

        speed -= (deccelerationSpeed + extraSpeedLoss) * brakeMultiplier * rollMultiplier * i * Time.deltaTime;
    }

    public IEnumerator Boost()
    {
        boosting = true;
        playerJump.Jumping = false;
        forward.forward = boostTransform.forward;
        forward.localRotation = new Quaternion(0, forward.localRotation.y, 0, forward.localRotation.w);
        yield return new WaitForSeconds(boostSec);
        boosting = false;
        playerJump.enabled = true;
    }

    public void BoostPad(float sec, Transform tr)
    {
        boostSec = sec;
        boostTransform = tr;
        //Not giving parameters to the IEnumerator because when i do it resumes but with a string it starts over when i stop it
        StartCoroutine("Boost");
    }

    public void StopBoost()
    {
        //Stopping the Ienumerator with a string actually stops it and doesn't pause it
        StopCoroutine("Boost");
    }
}
