using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject mainPanel;    // Panel 0
    public GameObject highPanel;    // Panel 1
    public GameObject instrPanel;   // Panel 2
    public GameObject creditsPanel; // Panel 3
    public GameObject levelPanel;   // Panel 4
    public GameObject diffPanel;    // Panel 5
    public GameObject scoreLevel1;  // Panel 5
    public GameObject scoreLevel2;  // Panel 5
    public GameObject scoreLevel3;  // Panel 5

    public UnityEngine.UI.Button bgmButton;
    public UnityEngine.UI.Button sfxButton;
    

    SceneController sceneController;

    private void Start()
    {
        sceneController = FindObjectOfType<SceneController>();
        ButtonToggle();
    }

    public void SetPanel(int selection)
    {
        switch (selection)
        {
            case 0: //Main Menu    
                mainPanel.SetActive(true);
                highPanel.SetActive(false);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(false);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(false);
                break;

            case 1: //High Scores
                mainPanel.SetActive(false);
                highPanel.SetActive(true);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(false);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(false);
                break;

            case 2: //How to Play
                mainPanel.SetActive(false);
                highPanel.SetActive(false);
                instrPanel.SetActive(true);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(false);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(false);
                break;

            case 3: //Credits
                mainPanel.SetActive(false);
                highPanel.SetActive(false);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(true);
                levelPanel.SetActive(false);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(false);
                break;

            case 4: //Level Selection
                mainPanel.SetActive(false);
                highPanel.SetActive(false);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(true);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(false);
                break;

            case 5: //Difficulty
                mainPanel.SetActive(false);
                highPanel.SetActive(false);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(false);
                diffPanel.SetActive(true);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(false);
                break;

            case 6: //Level 1 Highscores
                mainPanel.SetActive(false);
                highPanel.SetActive(false);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(false);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(true);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(false);
                break;

            case 7: //Level 2 Highscores
                mainPanel.SetActive(false);
                highPanel.SetActive(false);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(false);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(true);
                scoreLevel3.SetActive(false);
                break;

            case 8: //Level 3 Highscores
                mainPanel.SetActive(false);
                highPanel.SetActive(false);
                instrPanel.SetActive(false);
                creditsPanel.SetActive(false);
                levelPanel.SetActive(false);
                diffPanel.SetActive(false);
                scoreLevel1.SetActive(false);
                scoreLevel2.SetActive(false);
                scoreLevel3.SetActive(true);
                break;
        }

    }

    public void LevelSelection(int level)
    {
        sceneController.myLevel = level;
    }

    public void DifficultySelection(int diff)
    {
        sceneController.difficulty = diff;
        sceneController.LoadLevel(sceneController.myLevel);
    }

    public void ToggleBGM()
    {
        SceneController.isBgmMuted = !SceneController.isBgmMuted;
        sceneController.UpdateBGM();
        ButtonToggle();
    }

    public void ToggleSFX()
    {
        SceneController.isSfxMuted = !SceneController.isSfxMuted;
        sceneController.UpdateSFX();
        ButtonToggle();
    }

    public void ButtonToggle()
    {
        if (SceneController.isBgmMuted)
            bgmButton.GetComponent<Image>().color = Color.red;
        else if(!SceneController.isBgmMuted)
            bgmButton.GetComponent<Image>().color = Color.white;

        if(SceneController.isSfxMuted)
            sfxButton.GetComponent<Image>().color = Color.red;
        else if (!SceneController.isSfxMuted)
            sfxButton.GetComponent<Image>().color = Color.white;
    }

    public void EndGame()
    {
        Application.Quit();
    }

    
}
