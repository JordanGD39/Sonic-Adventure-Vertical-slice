using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;

    private bool selected = false;

    private void Start()
    {
        pauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(Constants.Inputs.pause))
        {
            if (!pauseUI.activeSelf)
            {
                pauseUI.SetActive(true);                
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(pauseUI.transform.GetChild(1).GetChild(1).gameObject);
                Time.timeScale = 0;
                AudioManager.instance.StopPlaying(AudioManager.instance.CurrSound.name);

            }
            else
            {
                Continue();
            }
        }
        
    }

    public void Continue()
    {
        pauseUI.SetActive(false);
        Time.timeScale = 1;
        AudioManager.instance.Play(AudioManager.instance.CurrSound.name);
    }

    public void Restart()
    {
        StartCoroutine(GameObject.FindGameObjectWithTag(Constants.Tags.player).GetComponent<PlayerDeath>().Die());
    }
}
