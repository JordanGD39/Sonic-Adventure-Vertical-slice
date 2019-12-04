using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameObject player;

    public GameObject Player { get { return player; } set { player = value; } }

    private float timer = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        AudioManager.instance.Play("TestBG");
        timer = 0;
        player = GameObject.FindGameObjectWithTag(Constants.Tags.player);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        float centiseconds = timer * 100;

        centiseconds = centiseconds % 100;

        Transform ui = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(1);

        ui.GetChild(1).GetComponent<Text>().text = minutes + ":" + seconds + ":" + centiseconds.ToString("00");

        if (player != null)
        {
            ui.GetChild(3).GetComponent<Text>().text = player.GetComponent<PlayerRingAmount>().RingAmount[0].ToString("000");
        }
    }
}
