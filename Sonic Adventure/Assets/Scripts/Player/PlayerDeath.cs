using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    private GameObject fadeObject;

    private void Start()
    {
        fadeObject = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.childCount - 1).gameObject;
        fadeObject.SetActive(false);
    }

    public IEnumerator Die()
    {
        Camera.main.GetComponent<ThirdPersonCameraControl>().Stop = true;
        fadeObject.SetActive(true);
        yield return new WaitForSeconds(2);        

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
