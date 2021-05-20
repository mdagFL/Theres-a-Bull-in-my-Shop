using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrintScores : MonoBehaviour
{
    private Transform scoreParent;
    private Transform scoreEntry;

    public int levelScores;

    void Start()
    {
        scoreParent = transform.Find("highscoreParent");
        scoreEntry = scoreParent.Find("highscoreEnter");
        scoreEntry.gameObject.SetActive(false);

        PopulateTable();
    }

    private void PopulateTable()
    {
        float height = 40f;

        OrganizeScores();

        int displayCount = 0;

        for (int i = 0; i < (SaveData.current.scores.Count); i++)
        {
            if(SaveData.current.scores[i].level == levelScores)
            {
                Transform scoreEntryTransform = Instantiate(scoreEntry, scoreParent);
                RectTransform scoreEntryRectTransform = scoreEntryTransform.GetComponent<RectTransform>();
                scoreEntryRectTransform.anchoredPosition = new Vector2(0, -height * (displayCount));
                scoreEntryTransform.gameObject.SetActive(true);

                scoreEntryTransform.Find("rankText").GetComponent<Text>().text = (displayCount+1).ToString();

                scoreEntryTransform.Find("nameText").GetComponent<Text>().text = SaveData.current.scores[i].name.ToString();

                scoreEntryTransform.Find("diffText").GetComponent<Text>().text = SaveData.current.scores[i].difficulty.ToString();

                scoreEntryTransform.Find("dateText").GetComponent<Text>().text = SaveData.current.scores[i].date.ToString("MM/dd/yy");

                scoreEntryTransform.Find("scoreText").GetComponent<Text>().text = SaveData.current.scores[i].score.ToString();

                displayCount++;
            }

            if(displayCount == 5)
                i = SaveData.current.scores.Count;
            

        }
    }

    private void OrganizeScores()
    {
        //Using Bubble Sort to order the highscores by the score
        for (int i = 0; i < SaveData.current.scores.Count; i++)
        {
            for (int j = i + 1; j < SaveData.current.scores.Count; j++)
            {
                if (SaveData.current.scores[i].score < SaveData.current.scores[j].score)
                {
                    ScoreEntry temp = SaveData.current.scores[i];
                    SaveData.current.scores[i] = SaveData.current.scores[j];
                    SaveData.current.scores[j] = temp;
                }
            }
        }
    }

}
