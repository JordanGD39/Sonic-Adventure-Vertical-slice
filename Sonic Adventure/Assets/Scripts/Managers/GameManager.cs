using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform ui;



    private float timer = 0;

    public bool StopTimer { get; set; } = false;
    public bool ChangeMusicOkay { get; set; } = false;
    public int Score { get; set; } = 0;
    public float Timer { get { return timer; } set { timer = value; } }

    public enum mode {AUTO, FREE}

    public mode cameraMode;

    [SerializeField] private int lives = 5;

    public int Lives { get { return lives; } set { lives = value; } }
    public bool Dying { get; set; } = false;
    public bool AlreadyBlinking { get; set; } = false;

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
        ChangeMusic();     
        
        timer = 0;
        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            ui = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(0);
            ui.GetChild(5).GetComponent<Text>().text = lives.ToString("00");
        }
    }

    private void Update()
    {
        if (!StopTimer)
        {
            timer += Time.deltaTime;
        }

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = (timer % 60).ToString("00");
        float centiseconds = timer * 100;

        centiseconds = centiseconds % 100;

        if (ui != null)
        {
            ui.GetChild(1).GetComponent<Text>().text = minutes + ":" + seconds + ":" + centiseconds.ToString("00");
        }
        else
        {
            if (SceneManager.GetActiveScene().buildIndex != 2)
            {
                ui = GameObject.FindGameObjectWithTag(Constants.Tags.canvas).transform.GetChild(0);
            }
        }

        if (player != null)
        {
            ui.GetChild(3).GetComponent<Text>().text = player.GetComponent<PlayerRingAmount>().RingAmount[0].ToString("000");

            if (ChangeMusicOkay)
            {
                ChangeMusic();
            }
        }
        else
        {
            player = GameObject.FindGameObjectWithTag(Constants.Tags.player);            
        }

        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            ui.GetChild(5).GetComponent<Text>().text = lives.ToString("00");
        }        
    }

    public IEnumerator RingBlink()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(Constants.Tags.player);
        }

        while (player.GetComponent<PlayerRingAmount>().RingAmount[0] == 0)
        {
            yield return new WaitForSeconds(0.5f);
            ui.GetChild(3).GetComponent<Text>().color = Color.red;
            yield return new WaitForSeconds(0.5f);
            ui.GetChild(3).GetComponent<Text>().color = Color.white;
        }

        ui.GetChild(3).GetComponent<Text>().color = Color.white;
    }

    private void ChangeMusic()
    {
        timer = 0;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                AudioManager.instance.Play("Stage1");
                break;
            case 1:
                AudioManager.instance.Play("TestBG");
                break;
            case 2:
                AudioManager.instance.Play("Credits");
                break;
        }

        ChangeMusicOkay = false;
    }
}
