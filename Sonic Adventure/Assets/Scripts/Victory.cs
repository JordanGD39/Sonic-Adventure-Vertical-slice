using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Victory : MonoBehaviour
{
    private Transform ui;

    private void Start()
    {
        ui = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(2).GetChild(2);
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
        Camera.main.GetComponent<AutoCamera>().Stop = true;
        Camera.main.GetComponent<ThirdPersonCameraControl>().Stop = true;
        other.transform.parent.LookAt(new Vector3(0, Camera.main.transform.position.y, 0));
        ui.parent.gameObject.SetActive(true);
        StartCoroutine("ShowResults");
    }

    private IEnumerator ShowResults()
    {
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(0).gameObject.SetActive(true);
        ui.GetChild(0).GetComponent<Text>().text = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(0).GetChild(1).GetComponent<Text>().text;
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(1).gameObject.SetActive(true);
        int rings = GameObject.FindGameObjectWithTag(Constants.Tags.player).GetComponent<PlayerRingAmount>().RingAmount[0];
        ui.GetChild(1).GetComponent<Text>().text = rings.ToString();
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(3).gameObject.SetActive(true);
        
        float minutes = Mathf.Floor(GameManager.instance.Timer / 60);
        float seconds = GameManager.instance.Timer % 60;

        float timeScore = 5000;

        if (minutes > 0)
        {
            timeScore -= minutes * 1000;

            timeScore += seconds * 10;
        }

        ui.GetChild(3).GetComponent<Text>().text = timeScore.ToString();
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(4).gameObject.SetActive(true);
        rings *= 10;
        ui.GetChild(4).GetComponent<Text>().text = rings.ToString();
        yield return new WaitForSeconds(0.5f);
        ui.GetChild(5).gameObject.SetActive(true);
        int totalScore = 0;

        int potentialTotalScore = Mathf.RoundToInt(timeScore);

        while (timeScore > 0)
        {
            timeScore -= 9;
            ui.GetChild(3).GetComponent<Text>().text = timeScore.ToString();
            totalScore += 9;
            ui.GetChild(5).GetComponent<Text>().text = totalScore.ToString();
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

        yield return new WaitForSeconds(3);
    }
}
