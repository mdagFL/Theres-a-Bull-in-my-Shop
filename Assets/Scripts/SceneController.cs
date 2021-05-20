using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SceneController : MonoBehaviour
{
    public int difficulty;
    public bool loadedGame;
    public int myScore;
    public int myLevel;
    Scene scene;
    public static bool isBgmMuted;
    public static bool isSfxMuted;
    public AudioMixer mixer;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        UpdateBGM();
        UpdateSFX();


    }

    public void UpdateBGM()
    {
        if (isBgmMuted)
            mixer.SetFloat("bgmVol", -80f);
        else
            mixer.SetFloat("bgmVol", -14.97f);
    }

    public void UpdateSFX()
    {
        if (isSfxMuted)
            mixer.SetFloat("sfxVol", -80f);
        else
            mixer.SetFloat("sfxVol", 0f);
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SaveData.LoadScores();
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Equals))
        {
            /*
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCount - 1)
                SceneManager.LoadScene(0);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            */
            SceneManager.LoadScene(scene.buildIndex + 1);
        }
        else if (Input.GetKeyUp(KeyCode.Minus))
        {
            /*
            if (SceneManager.GetActiveScene().buildIndex == 0)
                SceneManager.LoadScene(SceneManager.sceneCount - 1);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            */
            SceneManager.LoadScene(scene.buildIndex - 1);
        }
        
    }
    

    public void LoadLevel(int index)
    {
        // if a menu screen, unlock cursor
        if (index == 0 || index == 4)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        SceneManager.LoadScene(index);
    }
    
    public void Win(int score)
    {
        myScore = score + difficulty * 350;
        if(NewHighScore(myScore))
        {            
            UnlockCursor();
            SceneManager.LoadScene(4);
        }
        else
        {
            UnlockCursor();
            SceneManager.LoadScene(0);
        }
        
    }

    public bool NewHighScore(int score)
    {
        int count = 0;

        if (SaveData.current.scores.Count == 0)
            return true;

        for(int i = 0; i < SaveData.current.scores.Count; i++)
        {
            if(score < SaveData.current.scores[i].score && SaveData.current.scores[i].level == myLevel)
                count++;
        }

        if(count < 5)
        {
            count = 0;
            return true;
        }
        count = 0;
        return false;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0 && loadedGame == true)
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
            DestroyImmediate(this.gameObject);
        }
        else
            loadedGame = true;
        //scene = SceneManager.GetActiveScene();
    }

}
