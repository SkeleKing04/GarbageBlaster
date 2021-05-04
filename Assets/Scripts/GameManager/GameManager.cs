﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
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
    public string m_SettingsFileName = "settings.txt";
    public float[] m_SettingsValues = new float[2];
    public string m_HighScoresFileName = "highscores.txt";
    public string m_HighScoreNames = "highscorenames.txt";
    public int[] m_Scores = new int[5];
    public string[] m_ScoreNames = new string[5];
    public bool waiting = false;
    public float waitFor;
    //private bool wait = false;
    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Directory - " + currentDirectory);
        LoadSettings();
        LoadScores();
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
        SaveSettings();
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
    public void LoadSettings()
    {
        bool fileExists = File.Exists(currentDirectory + "\\" + m_SettingsFileName);
        if (fileExists == true)
        {
            Debug.Log(m_SettingsFileName + " exists");
        }
        else
        {
            Debug.Log(m_SettingsFileName + " does not exist", this);
            return;
        }
        StreamReader fileReader;
        try
        {
            fileReader = new StreamReader(currentDirectory + "\\" + m_SettingsFileName);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        m_SettingsValues = new float[m_SettingsValues.Length];
        int SettingsValueCount = 0;
        while (fileReader.Peek() != 0 && SettingsValueCount < m_SettingsValues.Length)
        {
            string fileLine = fileReader.ReadLine();
            float readValue = -1;
            bool didParse = float.TryParse(fileLine, out readValue);
            if (didParse)
            {
                m_SettingsValues[SettingsValueCount] = readValue;
            }
            else
            {
                Debug.Log("INVALID SETTINGS VALUE @ " + SettingsValueCount + ", USING DEFAULT VALUE.", this);
                m_SettingsValues[SettingsValueCount] = 0;
            }
            SettingsValueCount++;
        }
        fileReader.Close();
        Debug.Log("Settings Loaded. Applying...");
        m_VolumeSlider.value = m_SettingsValues[0];
        m_FOVSlider.value = m_SettingsValues[1];
    }
    public void SaveSettings()
    {
        m_SettingsValues[0] = m_VolumeSlider.value;
        m_SettingsValues[1] = m_FOVSlider.value;
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + m_SettingsFileName);
        for (int i = 0; i < m_SettingsValues.Length; i++)
        {
            fileWriter.WriteLine(m_SettingsValues[i]);
        }
        fileWriter.Close();
        Debug.Log("Settings Updated");
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
        AddScore();
        SaveScores();

    }
    IEnumerator WaitCommand(float waitFor)
    {
        Debug.Log("Waiting for " + waitFor + " seconds");
        yield return new WaitForSecondsRealtime(waitFor);

        Debug.Log("Done waiting");

    }
    public void LoadScores()
    {
        bool fileExists = File.Exists(currentDirectory + "\\" + m_HighScoresFileName);
        if (fileExists == true)
        {
            Debug.Log(m_HighScoresFileName + " exists");
        }
        else
        {
            Debug.Log(m_HighScoresFileName + " does not exist", this);
            return;
        }
        fileExists = File.Exists(currentDirectory + "\\" + m_HighScoreNames);
        if (fileExists == true)
        {
            Debug.Log(m_HighScoreNames + " exists");
        }
        else
        {
            Debug.Log(m_HighScoreNames + " does not exist", this);
            return;
        }
        StreamReader fileReader;
        try
        {
            fileReader = new StreamReader(currentDirectory + "\\" + m_HighScoresFileName);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        m_Scores = new int[m_Scores.Length];
        int ScoresCount = 0;
        Debug.Log("loading scores");
        while (fileReader.Peek() != 0 && ScoresCount < m_Scores.Length)
        {
            string fileLine = fileReader.ReadLine();
            Debug.Log("fileline " + fileLine);
            int readValue = -1;
            Debug.Log("readvalue " + readValue);
            bool didParse = int.TryParse(fileLine, out readValue);
            Debug.Log("didParse " + didParse);
            if (didParse)
            {
                m_Scores[ScoresCount] = readValue;
                Debug.Log("Score saved " + m_Scores[ScoresCount]);
            }
            else
            {
                Debug.Log("INVALID SETTINGS VALUE @ " + ScoresCount + ", USING DEFAULT VALUE.", this);
                m_Scores[ScoresCount] = 0;
            }
            ScoresCount++;
        }
        try
        {
            fileReader = new StreamReader(currentDirectory + "\\" + m_HighScoreNames);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
        m_ScoreNames = new string[m_ScoreNames.Length];
        int NameCount = 0;
        while (fileReader.Peek() != 0 && NameCount < m_ScoreNames.Length)
        {
            string fileLine = fileReader.ReadLine();
            Debug.Log("fileline " + fileLine);
            m_ScoreNames[NameCount] = fileLine;
            Debug.Log("name saved " + m_ScoreNames[NameCount]);
            NameCount++;
        }
        fileReader.Close();
        Debug.Log("Scores Loaded.");
    }
    public void SaveScores()
    {
        StreamWriter fileWriter = new StreamWriter(currentDirectory + "\\" + m_HighScoresFileName);
        for (int i = 0; i < m_Scores.Length; i++)
        {
            fileWriter.WriteLine(m_Scores[i]);
        }
        fileWriter = new StreamWriter(currentDirectory + "\\" + m_HighScoreNames);
        for (int i = 0; i < m_ScoreNames.Length; i++)
        {
            fileWriter.WriteLine(m_ScoreNames[i]);
        }
        fileWriter.Close();
        Debug.Log("scores and names saved");
    }
    public void AddScore()
    {
        // First up we find out what index it belongs at. // This will be the first index with a score lower than // the new score.
        int desiredIndex = -1;
        for (int i = 0; i < m_Scores.Length; i++)
        {
            // Instead of checking the value of desiredIndex // we could also use 'break' to stop the loop.
            if (m_Scores[i] < m_Score || m_Scores[i] == 0)
            {
                desiredIndex = i;
                break;
            }
        }
        // If no desired index was found then the score // isn't high enough to get on the table, so we just // abort.
        if (desiredIndex < 0)
        {
            Debug.Log("Score of " + m_Score + " is not high enough for high scores list.", this);
            return;
        }
        // Then we move all of the scores after that index // back by one position. We'll do this by looping from // the back of the array to our desired index.
        for (int i = m_Scores.Length - 1; i > desiredIndex; i--)
        {
            m_Scores[i] = m_Scores[i - 1];
        }
        // Insert our new score in its place
        m_Scores[desiredIndex] = m_Score;
        /*for (int i = m_ScoreNames.Length - 1; i > desiredIndex; i--)
        {
            m_ScoreNames[i] = m_ScoreNames[i - 1];
        }*/
        m_ScoreNames[desiredIndex] = m_PlayerName;
        Debug.Log(m_PlayerName + "'s score of " + m_Score + " entered into the high scores at position " + desiredIndex, this);
    }
}
