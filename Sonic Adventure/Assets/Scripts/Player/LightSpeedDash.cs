using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSpeedDash : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private Animator anim;
    private ModelManage modelManage;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Material sonicMat;
    [SerializeField] private Material sonicBallMat;
    [SerializeField] private Material lightMat;
    [SerializeField] private SkinnedMeshRenderer[] sonicMeshRenderers;
    [SerializeField] private MeshRenderer ballRenderer;
    [SerializeField] private ParticleSystem afterImage;
    [SerializeField] private float speed = 40; 
    [SerializeField] private float dashReleaseSpeed = 80; 
    [SerializeField] private float activationRingRange = 5;
    [SerializeField] private float dashingRingRange = 10;
    [SerializeField] private float followNextRingDistance = 10;
    [SerializeField] private float dashReleaseTime = 1;

    public bool LightSpeedDashReady { get; set; } = false;
    private bool dashing = false;
    public bool Dashing { get { return dashing; } }
    private Transform previousRing;
    private Transform currentRing;
    private bool followRings = false;
    private bool releaseDash = false;

    // Start is called before the first frame update
    void Start()
    {
        ChangeMeshMaterial(false);
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
        anim = GetComponentInChildren<Animator>();
        modelManage = GetComponent<ModelManage>();
    }

    private void Update()
    {
        if (playerMovement.Boosting && dashing)
        {
            ResetDash();
            return;
        }

        if (LightSpeedDashReady && !dashing && Input.GetButtonUp("Spindash"))
        {
            dashing = true;
            StartLightSpeedDash();
        }

        if (followRings)
        {
            GoTowardsRing(currentRing);
        }

        if (releaseDash)
        {
            rb.velocity = transform.GetChild(0).forward * dashReleaseSpeed;
        }
    }

    public void ChangeMeshMaterial(bool light)
    {
        foreach (SkinnedMeshRenderer rend in sonicMeshRenderers)
        {
            rend.material = light ? lightMat : sonicMat;
        }

        ballRenderer.material = light ? lightMat : sonicBallMat;
    }

    public void StartLightSpeedDash()
    {
        AudioManager.instance.Play("Go");
        anim.SetBool("LightSpeedDashing", true);
        afterImage.Play();
        modelManage.ChangeToNormalModel();
        playerMovement.Speed = 0;
        CheckRingsInRange();
    }

    // Update is called once per frame
    private void CheckRingsInRange()
    {
        Collider[] rings = Physics.OverlapSphere(transform.position, currentRing != null ? dashingRingRange : activationRingRange, layerMask);
        Debug.Log(rings.Length);

        if (rings.Length > 0)
        {
            FindClosestRing(rings);            
        }
        else
        {
            ReleaseDash();
        }
    }

    private void FindClosestRing(Collider[] rings)
    {
        float closestDistance = 999;
        Transform closestRing = rings[0].transform;

        for (int i = 0; i < rings.Length; i++)
        {
            Transform ring = rings[i].transform;
            float distance = Vector3.Distance(ring.position, transform.position);

            if (distance < closestDistance && ring != previousRing)
            {
                closestRing = ring;
                closestDistance = distance;
            }
        }

        if (closestDistance < 999)
        {
            currentRing = closestRing;
            followRings = true;
        }
        else
        {
            ReleaseDash();
        }        
    }

    private void GoTowardsRing(Transform ring)
    {
        if (ring == null)
        {
            ReleaseDash();
            return;
        }

        rb.velocity = Vector3.zero;
        transform.position = Vector3.MoveTowards(transform.position, ring.position, speed * Time.deltaTime);
        transform.GetChild(0).LookAt(ring);

        if (Vector3.Distance(transform.position, ring.position) < followNextRingDistance)
        {
            previousRing = ring;
            CheckRingsInRange();
        }
    }

    private void ReleaseDash()
    {
        followRings = false;
        previousRing = null;
        if (currentRing == null)
        {
            anim.SetBool("LightSpeedDashing", true);
            afterImage.Play();
            releaseDash = true;
            AudioManager.instance.Play("Go");
            Invoke(nameof(ResetDash), dashReleaseTime);
        }
        else
        {
            ResetDash();
        }

        currentRing = null;        
    }

    public void ResetDash()
    {
        if (!dashing)
        {
            return;
        }

        ChangeMeshMaterial(false);        
        LightSpeedDashReady = false;
        dashing = false;

        float releaseSpeed = releaseDash ? dashReleaseSpeed : speed;

        playerMovement.Speed = releaseSpeed;
        rb.velocity = transform.GetChild(0).forward * releaseSpeed;
        releaseDash = false;
        afterImage.Stop();
        anim.SetBool("LightSpeedDashing", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, currentRing != null ? dashingRingRange : activationRingRange);
    }
}
