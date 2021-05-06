using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public highscoreEditor m_HighScoreEditor;
    public settingEditor m_SettingEditor;
    public int m_Score = 0;
    public string m_PlayerName;
    public Text m_ScoreDisplay;
    public Text m_GarbageLoadedDisplay;
    public GameObject[] m_InGameHUD;
    public GameObject[] m_PauseMenu;
    public bool m_isGamePaused = false;
    public Component[] m_PlayerParts;
    public Slider m_VolumeSlider;
    public Text m_VolumeSliderText;
    public Slider m_FOVSlider;
    public Text m_FOVSliderText;
    public float m_LevelTime;
    public float m_DisplayTime;
    public Text m_TimerDisplay;
    public Text m_MessageText;
    public bool m_GameOver = false;
    string currentDirectory;
    public bool waiting = false;
    public float waitFor;
    //private bool wait = false;
    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Directory - " + currentDirectory);
        m_SettingEditor.LoadSettings(m_VolumeSlider, m_FOVSlider);
        m_HighScoreEditor.LoadScores();
        ChangeVolume();
        ChangeFOV();
        m_GameOver = false;
        m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
        VacGun vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
        m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
        foreach (GameObject gameObject in m_PauseMenu)
        {
            gameObject.gameObject.SetActive(false);
        }
        m_MessageText.gameObject.SetActive(false);
        AudioListener.volume = m_VolumeSlider.value;
        m_VolumeSliderText.text = "Volume: " + Mathf.RoundToInt(m_VolumeSlider.value * 100).ToString() + "%";
        m_FOVSliderText.text = "FOV: " + Mathf.RoundToInt((m_FOVSlider.value * 100) + 30).ToString();
        m_DisplayTime = m_LevelTime;
        //m_MessageText.text = "Ready...";
        //Time.timeScale = 0;
        //waitFor = 3;
        //StartCoroutine(WaitCommand(waitFor));
        //WaitUntil waitUntil = wait == true;
        //m_MessageText.text = "Go!";
        //StartCoroutine(Timer(m_DisplayTime, m_TimerDisplay));
        //waitFor = 0.5f;
        //StartCoroutine(WaitCommand(waitFor));
        m_MessageText.gameObject.SetActive(false);
        //Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab) && m_isGamePaused == false)
        {
            PauseGame();
        }
        else if (Input.GetKeyUp(KeyCode.Tab) && m_isGamePaused == true)
        {
            ResumeGame();
        }
        UpdateTimer();
    }
    public void UpdateScoreText()
    {
        m_ScoreDisplay.text = "Score:\n"+m_Score.ToString();
    }
    public void UpdateGarbageText()
    {
        VacGun vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
        m_GarbageLoadedDisplay.text = "Ammo:\n"+vacGun.m_LoadedGarbage.ToString();
    }
    public void QuitGame()
    {
        Debug.Log("Quiting... Loser!");
        Application.Quit();
    }
    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        foreach (GameObject gameObject in m_InGameHUD)
        {
            gameObject.gameObject.SetActive(false);
        }
        foreach (GameObject gameObject in m_PauseMenu)
        {
            gameObject.gameObject.SetActive(true);
        }
        Time.timeScale = 0;
        m_isGamePaused = true;
        foreach (Component part in m_PlayerParts)
        {
            part.gameObject.SetActive(false);
        }
    }
    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        foreach (GameObject gameObject in m_InGameHUD)
        {
            gameObject.gameObject.SetActive(true);
        }
        foreach (GameObject gameObject in m_PauseMenu)
        {
            gameObject.gameObject.SetActive(false);
        }
        Time.timeScale = 1;
        m_isGamePaused = false;
        foreach(Component part in m_PlayerParts)
        {
            part.gameObject.SetActive(true);
        }
        m_SettingEditor.SaveSettings(m_VolumeSlider, m_FOVSlider);
    }
    public void ChangeVolume()
    {
        AudioListener.volume = m_VolumeSlider.value;
        m_VolumeSliderText.text = "Volume: " + Mathf.RoundToInt(m_VolumeSlider.value * 100).ToString() + "%";
    }
    public void ChangeFOV()
    {
        Camera.main.fieldOfView = ((m_FOVSlider.value * 100) + 30);
        m_FOVSliderText.text = "FOV: " + Mathf.RoundToInt((m_FOVSlider.value * 100) + 30).ToString();
    }
    public void UpdateTimer()
    {
        m_DisplayTime -= Time.deltaTime;
        int seconds = Mathf.RoundToInt(m_DisplayTime);
        m_TimerDisplay.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));
        if (m_DisplayTime <= 0)
        {
            while (m_GameOver == false)
            {
                GameOver();
                m_GameOver = true;
            }
        }
        
    }
    /*IEnumerator Timer(float m_DisplayTime, Text m_TimerDisplay)
    {
        while (true)
        {
            m_DisplayTime -= Time.deltaTime;
            int seconds = Mathf.RoundToInt(m_DisplayTime);
            m_TimerDisplay.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));
            if (m_DisplayTime <= 0)
            {
                GameOver();
            }
        }
    }*/
    public void GameOver()
    {
        m_MessageText.gameObject.SetActive(true);
        Debug.Log("Game Over");
        PauseGame();
        m_isGamePaused = false;
        m_PauseMenu[1].SetActive(false);
        m_MessageText.text = "Time's Up!";
        //waitFor = 2;
        //StartCoroutine(WaitCommand(waitFor));
        m_MessageText.text = "Your final score was " + m_Score + "!\nWell done!";
        m_HighScoreEditor.AddScore(m_Score , m_PlayerName);
        m_HighScoreEditor.SaveScores();

    }
    IEnumerator WaitCommand(float waitFor)
    {
        Debug.Log("Waiting for " + waitFor + " seconds");
        yield return new WaitForSecondsRealtime(waitFor);

        Debug.Log("Done waiting");

    }
}
