using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggers : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _triggers;

    [SerializeField]
    private Animator _orcaAnimator;

    private bool triggered;

    public bool Triggered { get { return triggered; } set { value = triggered; } }

    private void Start()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == Constants.Tags.trigger && collision.gameObject.name != _triggers[0].name)
        {
            triggered = true;
        }
        else if (collision.gameObject.name == _triggers[0].name)
        {
            //Debug.Log("Triggered");
            _orcaAnimator.SetTrigger("Player Arrival");
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
