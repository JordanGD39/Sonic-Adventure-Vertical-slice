using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _triggers;

    [SerializeField]
    private Animator _orcaAnimator;

    [SerializeField]
    private Animator _waveAnimator;

    private Collider collidingObj;
    private Vector3 currentVel;
    private bool triggered;

    public Collider CollidingObj { get { return collidingObj; } set { value = collidingObj; } }
    public Vector3 CurrentVel { get { return currentVel; } set { value = currentVel; } }
    public bool Triggered { get { return triggered; } set { value = triggered; } }

    private void Start()
    {
        triggered = false;

        collidingObj = GetComponentInChildren<Collider>();
        currentVel = GetComponent<Rigidbody>().velocity;
}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == Constants.Tags.trigger && collision.gameObject.name != _triggers[0].name)
        {
            collidingObj = collision;
            currentVel = GetComponent<Rigidbody>().velocity;
            triggered = true;
        }
        else if (collision.gameObject.name == _triggers[0].name)
        {
            _orcaAnimator.SetTrigger("Player Arrival");
        }
        else if (collision.gameObject.name == _triggers[1].name)
        {
            _waveAnimator.SetTrigger("Player Arrival");
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == Constants.Tags.trigger && collision.gameObject.name != _triggers[0].name)
        {
            triggered = false;
        }
    }
}
