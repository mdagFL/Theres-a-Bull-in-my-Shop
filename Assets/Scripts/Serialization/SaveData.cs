using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public enum Difficulty
{
    easy, medium, hard
}

[System.Serializable]
public struct ScoreEntry
{
    public string name;
    public Difficulty difficulty;
    public System.DateTime date;
    public int score;
    public int level;
    public ScoreEntry(string a_name, Difficulty a_diff, System.DateTime a_date, int a_score, int a_level)
    {
        name = a_name;
        difficulty = a_diff;
        date = a_date;
        score = a_score;
        level = a_level;
    }
};


[System.Serializable]
public class SaveData
{
    private static SaveData _current;
    
    public List<ScoreEntry> scores;
    public static SaveData current
    {
        get
        {
            if (_current == null)
                _current = new SaveData();
            return _current;
        }
        
        set
        {
            if (value != null)
             _current = value;
        }
    }


    public SaveData()
    {
        scores = new List<ScoreEntry>();
        current = this;
        LoadScores();   
    }

    SaveData(List<ScoreEntry> a_scores)
    {
        this.scores = a_scores;
        current = this;
    }
    
    // add a new score and overwrite old scores file
    public static bool SaveScore(ScoreEntry score)
    {       
        string scoresPath = Application.persistentDataPath + "/score/scores.dat";
        string scoresDirectory = Application.persistentDataPath + "/score";

        BinaryFormatter formatter = new BinaryFormatter();
        current.scores.Add(score);
        if (!Directory.Exists(scoresDirectory))
        {
            Directory.CreateDirectory(scoresDirectory);
        }

        FileStream fs = File.Create(scoresPath);

        try
        {
            formatter.Serialize(fs, current);
            fs.Close();

            return true;
        }
        catch (IOException ex)
        {
            Debug.LogError(ex);
            current.scores.Remove(score);
            fs.Close();
            return false;
        }        
    }

    public static SaveData LoadScores()
    {
        string scoresPath = Application.persistentDataPath + "/score/scores.dat";

        if (!File.Exists(scoresPath))
        {
            return null;
        }

        BinaryFormatter formatter = new BinaryFormatter();
        object data;
        FileStream fs = File.Open(scoresPath, FileMode.Open);
        try
        {
            data = formatter.Deserialize(fs);
            fs.Close();

            SaveData sd = (SaveData)data;

            current = (SaveData)data;

            return (SaveData)data;            
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
            fs.Close();
            return null;
        }
    }

}
