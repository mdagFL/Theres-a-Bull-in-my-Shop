using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Text hp;
    public Text time;
    public float maxTime;
    public float timeCounter;
    private bool timeOut;
    public Image crosshair;
    public bool crossLock = false;
    public bool levelEnding;
    private bool playerWon;
    public float endingTimer;
    public Player player;
    public GameObject statsPanel;
    public GameObject pausePanel;
    public GameObject wlPanel;
    private SceneController myController;

    // Start is called before the first frame update
    void Start()
    {
        maxTime = player.timeLimit;
        timeOut = false;
        timeCounter = maxTime;
        hp.text = "HP: " + player.MaxHP;
        myController = FindObjectOfType<SceneController>();
        levelEnding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (levelEnding)
        {
            endingTimer -= Time.deltaTime;
            if (endingTimer <= 0)
            {
                if (playerWon)
                    myController.Win(player.CalculateScore());
                else
                    myController.LoadLevel(0);
            }
         
        }
        if (timeOut == false)
        {
            if(timeCounter > 0.1f)
            {
                timeCounter -= Time.deltaTime;
                time.text = "Time: " + string.Format("{0:00}:{1:00}", Mathf.FloorToInt(timeCounter / 60), Mathf.FloorToInt(timeCounter % 60));
            }
            else
            {
                time.text = "Time has expired!";
                timeCounter = 0;
                timeOut = true;
                Lose();
            }
        }      
    }

    public void UpdateHP(int playerHP)
    {
        hp.text = "HP: " + playerHP;
    }

    public void ChangeCrosshair(Color32 color)
    {
        if(crossLock == false)
            crosshair.color = color;
    }

    public void PauseToggle()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            statsPanel.SetActive(true);
            pausePanel.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0f;
            statsPanel.SetActive(false);
            pausePanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ReturnToMenu()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }
            SceneController controller = FindObjectOfType<SceneController>();
        controller.LoadLevel(0);
    }

    public void Win()
    {
        if (!levelEnding)
        {
            wlPanel.transform.GetChild(0).gameObject.SetActive(true);
            playerWon = true;
            EndLevel();
        }
    }

    public void Lose()
    {
        if (!levelEnding)
        {
            wlPanel.transform.GetChild(1).gameObject.SetActive(true);
            levelEnding = true;
            EndLevel();
        }
    }

    public void EndLevel()
    {
        levelEnding = true;
        statsPanel.SetActive(false);
        pausePanel.SetActive(false);
        wlPanel.SetActive(true);
    }
}
