using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    private GameObject fadeObjectOut;
    private Transform ui;

    [SerializeField]
    private int levelIndex = 0;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(2).GetChild(2);
        fadeObjectOut = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.childCount - 1).gameObject;
        ui.parent.gameObject.SetActive(false);
        for (int i = 0; i < ui.childCount; i++)
        {
            ui.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.Tags.playerCol))
        {
            GameObject.FindGameObjectWithTag(Constants.Tags.canvas).GetComponent<Pause>().enabled = false;
            GameManager.instance.StopTimer = true;
            StartCoroutine(WaitUntilPlayerIsGrounded(other));
        }
    }

    private IEnumerator WaitUntilPlayerIsGrounded(Collider other)
    {
        other.GetComponentInParent<PlayerJump>().enabled = false;
        other.transform.parent.GetChild(0).gameObject.SetActive(true);
        other.transform.parent.GetChild(1).gameObject.SetActive(false);

        if (other.GetComponent<SphereCollider>() != null)
        {
            other = other.transform.parent.GetChild(0).GetComponent<BoxCollider>();
        }

        while (!other.GetComponentInParent<PlayerMovement>().Grounded)
        {
            other.GetComponentInParent<PlayerMovement>().Movement = new Vector3(0, 0, 0);
            other.GetComponentInParent<Rigidbody>().velocity = new Vector3(0, other.GetComponentInParent<Rigidbody>().velocity.y, 0);
            yield return null;
        }

        other.transform.GetComponentInChildren<Animator>().SetBool("Win", true);        
        other.GetComponentInParent<PlayerMovement>().enabled = false;        
        other.transform.GetComponentInChildren<Animator>().SetBool("Grounded", true);
        other.GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        if (Camera.main.GetComponent<AutoCamera>() != null)
        {
            Camera.main.GetComponent<AutoCamera>().Stop = true;
        }
        Camera.main.GetComponent<ThirdPersonCameraControl>().Stop = true;
        other.transform.parent.LookAt(new Vector3(0, Camera.main.transform.position.y, 0));
        AudioManager.instance.StopPlaying(AudioManager.instance.CurrSound.name);
        AudioManager.instance.Play("Victory");        
        AudioManager.instance.Play("Yes");        
        yield return new WaitForSeconds(4.5f);
        ui.parent.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        other.GetComponentInParent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        StartCoroutine("ShowResults");
    }

    private IEnumerator ShowResults()
    {
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(0).gameObject.SetActive(true);
        AudioManager.instance.Play("Bang");
        ui.GetChild(0).GetComponent<Text>().text = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(0).GetChild(1).GetComponent<Text>().text;
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(1).gameObject.SetActive(true);
        AudioManager.instance.Play("Bang");
        int rings = GameObject.FindGameObjectWithTag(Constants.Tags.player).GetComponent<PlayerRingAmount>().RingAmount[0];
        ui.GetChild(1).GetComponent<Text>().text = rings.ToString();
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(2).gameObject.SetActive(true);
        AudioManager.instance.Play("Bang");
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(3).gameObject.SetActive(true);
        AudioManager.instance.Play("Bang");

        float minutes = Mathf.Floor(GameManager.instance.Timer / 60);
        float seconds = GameManager.instance.Timer % 60;

        float timeScore = 5000;

        if (minutes > 0)
        {
            timeScore -= minutes * 1000;

            timeScore += seconds * 10;
        }

        timeScore = Mathf.Round(timeScore);

        ui.GetChild(3).GetComponent<Text>().text = timeScore.ToString();
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(4).gameObject.SetActive(true);
        AudioManager.instance.Play("Bang");
        rings *= 10;
        ui.GetChild(4).GetComponent<Text>().text = rings.ToString();
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(5).gameObject.SetActive(true);
        int totalScore = GameManager.instance.Score;

        int potentialTotalScore = Mathf.RoundToInt(timeScore) + totalScore;

        while (timeScore > 0)
        {            
            timeScore -= 9;
            ui.GetChild(3).GetComponent<Text>().text = timeScore.ToString();
            totalScore += 9;
            ui.GetChild(5).GetComponent<Text>().text = totalScore.ToString();
            if (Input.GetButtonDown(Constants.Inputs.submit))
            {
                timeScore = 0;
                totalScore = potentialTotalScore;
            }
            yield return null;
        }

        if (timeScore < 0)
        {
            timeScore = 0;
        }

        if (totalScore > potentialTotalScore)
        {
            totalScore = potentialTotalScore;
        }

        ui.GetChild(3).GetComponent<Text>().text = timeScore.ToString();
        ui.GetChild(5).GetComponent<Text>().text = totalScore.ToString();

        potentialTotalScore += rings;

        while (rings > 0)
        {            
            rings -= 3;
            ui.GetChild(4).GetComponent<Text>().text = rings.ToString();
            totalScore += 3;
            ui.GetChild(5).GetComponent<Text>().text = totalScore.ToString();
            if (Input.GetButtonDown(Constants.Inputs.submit))
            {
                rings = 0;
                totalScore = potentialTotalScore;
            }
            yield return null;
        }

        if (rings < 0)
        {
            rings = 0;
        }

        if (totalScore > potentialTotalScore)
        {
            totalScore = potentialTotalScore;
        }

        ui.GetChild(4).GetComponent<Text>().text = rings.ToString();
        ui.GetChild(5).GetComponent<Text>().text = totalScore.ToString();

        GameManager.instance.Score = totalScore;

        fadeObjectOut.SetActive(true);

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(levelIndex);
        GameObject.FindGameObjectWithTag(Constants.Tags.canvas).GetComponent<Pause>().AudioGo = false;
        GameManager.instance.ChangeMusicOkay = true;   
    }
}
