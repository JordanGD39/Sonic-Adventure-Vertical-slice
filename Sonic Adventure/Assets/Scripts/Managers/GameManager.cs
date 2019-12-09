using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject player;

    private float timer = 0;

    [SerializeField] private int lives = 5;

    public int Lives { get { return lives; } set { lives = value; } }
    public bool Dying { get; set; } = false;

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
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                AudioManager.instance.Play("Stage1");
                break;
            case 1:
                AudioManager.instance.Play("TestBG");
                break;
        }
        
        timer = 0;
        player = GameObject.FindGameObjectWithTag(Constants.Tags.player);
        Transform ui = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(0);
        ui.GetChild(5).GetComponent<Text>().text = lives.ToString("00");
    }

    private void Update()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        float centiseconds = timer * 100;

        centiseconds = centiseconds % 100;

        Transform ui = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(0);

        ui.GetChild(1).GetComponent<Text>().text = minutes + ":" + seconds + ":" + centiseconds.ToString("00");

        if (player != null)
        {
            ui.GetChild(3).GetComponent<Text>().text = player.GetComponent<PlayerRingAmount>().RingAmount[0].ToString("000");
        }
        else
        {
            player = GameObject.FindGameObjectWithTag(Constants.Tags.player);            
        }

        ui.GetChild(5).GetComponent<Text>().text = lives.ToString("00");
    }
}
