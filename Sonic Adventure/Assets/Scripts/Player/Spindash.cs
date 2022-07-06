using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spindash : MonoBehaviour
{
    private TrailRenderer trailRenderer;
    
    [SerializeField] private GameObject rollModel;
    [SerializeField] private Animator spindashAnim;
    [SerializeField] private Material mat;
    [SerializeField] private float spinCharge = 0;
    [SerializeField] private float maxSpinCharge = 30;
    [SerializeField] private float minSpinCharge = 5;
    [SerializeField] private float maxSpinSpeed = 60;
    [SerializeField] private float chargeIncrease = 2;
    [SerializeField] private float speedLoss = 20;
    [SerializeField] private float minChargeTime = 0.5f;
    [SerializeField] private float slopeMultiplier = 1.5f;
    [SerializeField] private float deccMultiplier = 0.75f;
    [SerializeField] private float slopeThreshold = 5;
    public float SlopeMultiplier { get { return slopeMultiplier; } }
    public float DeccMultiplier { get { return deccMultiplier; } }
    public float SlopeThreshold { get { return slopeThreshold; } }

    [SerializeField] private bool spinning = false;
    public bool Spinning { get { return spinning; } }
    [SerializeField] private bool inBall = false;
    public bool InBall { get { return inBall; } }
    public bool SpindashStarted { get; set; } = false;
    private bool chargePressed = false;
    private float timer = 0;

    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private LightSpeedDash lightSpeedDash;
    private ModelManage modelManage;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
        lightSpeedDash = GetComponent<LightSpeedDash>();
        modelManage = GetComponent<ModelManage>();
        spinCharge = minSpinCharge;
        trailRenderer = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerMovement.Grounded || playerMovement.Boosting || lightSpeedDash.LightSpeedDashReady)
        {
            if (spinning || inBall)
            {
                StopRolling();
            }

            return;
        }

        if (playerMovement.Speed < 1 && inBall && playerMovement.HitAngle < slopeThreshold)
        {
            StopRolling();

            return;
        }

        CheckInput();

        if (SpindashStarted)
        {
            if (timer < 0.1f)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                SpindashStarted = false;
            }
        }

        if (inBall)
        {
            rollModel.transform.GetChild(0).Rotate(playerMovement.Speed * 100 * Time.deltaTime, 0, 0);
        }
    }

    private void CheckInput()
    {
        if (Input.GetButton("Spindash") && !inBall)
        {
            ChargeSpinDash();
        }
        else if (Input.GetButtonUp("Spindash"))
        {
            if (!inBall)
            {
                ReleaseSpinDash();
            }
            else
            {
                StopRolling();
            }
        }
    }

    private void ChargeSpinDash()
    {
        if (!spinning)
        {
            spindashAnim.ResetTrigger("LightSpeedChange");
            AudioManager.instance.Play("Spindash");
        }

        if (playerMovement.Speed > 0)
        {
            playerMovement.Speed -= speedLoss * Time.deltaTime;
        }
        else
        {
            playerMovement.Speed = 0;
        }

        float chargePercent = spinCharge / maxSpinCharge;

        spindashAnim.speed = chargePercent + 1;

        if (chargePercent > 0.6f)
        {
            spindashAnim.SetTrigger("LightSpeedChange");
        }

        if (spinCharge < maxSpinCharge)
        {
            spinCharge += chargeIncrease * Time.deltaTime;
        }
        else
        {
            AudioManager.instance.Play("Ready");
            lightSpeedDash.LightSpeedDashReady = true;
            lightSpeedDash.ChangeMeshMaterial(true);
            StopRolling();
            return;
        }

        spinning = true;
        modelManage.ChangeToSpinModel();
        inBall = false;
    }

    private void ReleaseSpinDash()
    {
        float speed = playerMovement.Speed + spinCharge;

        if (speed > maxSpinSpeed)
        {
            speed = maxSpinSpeed;
        }

        SpindashStarted = true;

        spinCharge = minSpinCharge;
        inBall = true;
        spinning = false;
        playerMovement.Speed = speed;
        modelManage.ChangeToBallModel();
        rollModel.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        rollModel.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        trailRenderer.emitting = true;
        AudioManager.instance.StopPlaying("Spindash");
        AudioManager.instance.Play("BoostPad");
        playerJump.StartBallBlink();
    }

    private void StopRolling()
    {
        AudioManager.instance.StopPlaying("Spindash");
        trailRenderer.emitting = false;
        spinning = false;
        SpindashStarted = false;
        inBall = false;
        spinCharge = minSpinCharge;
        if (playerJump.Jumping)
        {
            modelManage.ChangeToBallModel();
        }
        else
        {
            modelManage.ChangeToNormalModel();
        }
    }
}
