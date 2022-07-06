using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private GameObject fadeObjectOut;
    private GameObject fadeObjectIn;
    private Animator anim;

    private void Start()
    {
        fadeObjectOut = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.childCount - 1).gameObject;
        fadeObjectIn = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.childCount - 2).gameObject;
        fadeObjectIn.SetActive(true);
        fadeObjectOut.SetActive(false);
        GameManager.instance.Dying = false;
        GameManager.instance.StopTimer = false;
        anim = transform.GetComponentInChildren<Animator>();
        anim.SetBool("Dead", false);        
    }

    public IEnumerator Die()
    {
        AudioManager.instance.Play("No");
        GameManager.instance.StopTimer = true;
        anim.SetBool("Dead", true);
        GetComponent<PlayerJump>().enabled = false;
        if (Camera.main.GetComponent<AutoCamera>() != null)
        {
            Camera.main.GetComponent<AutoCamera>().Stop = true;
        }        
        Camera.main.GetComponent<ThirdPersonCameraControl>().Stop = true;
        fadeObjectOut.SetActive(true);
        GameManager.instance.Dying = true;        
        GetComponent<PlayerMovement>().Movement = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        yield return new WaitForSeconds(3);

        if (GameManager.instance.Lives > 0)
        {
            GameManager.instance.Lives--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            GameManager.instance.StopTimer = false;
        }
        else if (GameManager.instance.Lives <= 0)
        {
            Application.Quit();
        }
    }
}
