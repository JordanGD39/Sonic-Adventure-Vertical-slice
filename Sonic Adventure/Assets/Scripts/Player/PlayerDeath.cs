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
        anim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        anim.SetBool("Dead", false);        
    }

    public IEnumerator Die()
    {
        anim.SetBool("Dead", true);
        Camera.main.GetComponent<AutoCamera>().Stop = true;
        fadeObjectOut.SetActive(true);
        GameManager.instance.Dying = true;        
        yield return new WaitForSeconds(3);

        if (GameManager.instance.Lives > 0)
        {
            GameManager.instance.Lives--;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (GameManager.instance.Lives <= 0)
        {
            GameManager.instance.Lives = 5;
            SceneManager.LoadScene(0);
        }
    }
}
