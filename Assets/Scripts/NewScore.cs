using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewScore : MonoBehaviour
{
    InputField input;
    public Text scoreText;
    SceneController sceneController;
    Difficulty tempDiff;


    void Start()
    {
        input = FindObjectOfType<InputField>();
        sceneController = FindObjectOfType<SceneController>();
        SetScoreText();

    }

    void SetScoreText()
    {
        if(sceneController != null)
            scoreText.text = sceneController.myScore.ToString();
    }

    // Update is called once per frame
    public void SubmitScore()
    {
        Debug.Log(sceneController.difficulty);
        switch (sceneController.difficulty)
        {
            case 0:
                tempDiff = Difficulty.easy;
                break;
            case 1:
                tempDiff = Difficulty.medium;
                break;
            case 2:
                tempDiff = Difficulty.hard;
                break;
        }

        ScoreEntry highscore = new ScoreEntry((input.text != null ? input.text : "YOU"), tempDiff, System.DateTime.Now, sceneController.myScore, sceneController.myLevel);
        
        
        SaveData.SaveScore(highscore);
        sceneController.LoadLevel(0);
    }
}
