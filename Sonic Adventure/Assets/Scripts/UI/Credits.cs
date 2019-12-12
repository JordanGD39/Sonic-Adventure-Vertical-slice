using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private Animator anim;
    private Animator fade;

    private void Start()
    {
        anim = GetComponent<Animator>();
        fade = transform.parent.GetChild(4).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(Constants.Inputs.submit))
        {
            anim.speed = 20;
        }
        else
        {
            anim.speed = 1;
        }
    }

    public void ShowButtons()
    {
        fade.Play("ButtonsFade");
        EventSystem.current.SetSelectedGameObject(transform.parent.GetChild(0).gameObject);
        if (GameManager.instance.Score >= 5380)
        {
            transform.parent.GetChild(2).GetComponent<Text>().text = "You're too cool!";            
        }
        transform.parent.GetChild(3).GetComponent<Text>().text = "Score: " + GameManager.instance.Score;
    }

    public void Restart()
    {
        AudioManager.instance.StopPlaying("Credits");
        transform.parent.GetComponent<Pause>().AudioGo = false;
        GameManager.instance.Score = 0;
        SceneManager.LoadScene(0);        
        GameManager.instance.ChangeMusicOkay = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
