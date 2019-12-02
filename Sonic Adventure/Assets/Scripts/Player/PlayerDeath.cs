using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private GameObject fadeObject;

    private void Start()
    {
        fadeObject = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(0).gameObject;
        fadeObject.SetActive(false);
    }

    public IEnumerator Die()
    {
        Camera.main.GetComponent<ThirdPersonCameraControl>().Stop = true;
        fadeObject.SetActive(true);
        yield return new WaitForSeconds(2);
        AudioManager.instance.StopPlaying(AudioManager.instance.CurrSound.name);
        AudioManager.instance.Play(AudioManager.instance.CurrSound.name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
