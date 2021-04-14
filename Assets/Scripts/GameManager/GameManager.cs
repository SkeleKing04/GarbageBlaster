using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
    public int m_Score;
    public Text m_ScoreDiplay;
    // Start is called before the first frame update
    void Start()
    {
        m_ScoreDiplay.text = 0.ToString();
    }
=======
    public int m_Score = 0;
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
    private float m_DisplayTime;
    public Text m_TimerDisplay;
    public Text m_MessageText;
    string currentDirectory;
    public string m_SettingsFileName = "settings.txt";
    public float[] m_SettingsValues = new float[2];
    //private float waitFor;
    //private bool wait = false;
    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Directory - " + currentDirectory);
        LoadSettings();
        ChangeVolume();
        ChangeFOV();
        m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
        VacGun vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
        m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
        foreach (GameObject gameObject in m_PauseMenu)
        {
            gameObject.gameObject.SetActive(false);
        }
        AudioListener.volume = m_VolumeSlider.value;
        m_VolumeSliderText.text = "Volume: " + Mathf.RoundToInt(m_VolumeSlider.value * 100).ToString() + "%";
        m_FOVSliderText.text = "FOV: " + Mathf.RoundToInt((m_FOVSlider.value * 100) + 30).ToString();
        m_DisplayTime = m_LevelTime;
        m_MessageText.text = "Ready...";
        Time.timeScale = 0;
        //waitFor = 3;
        //StartCoroutine(WaitCommand(waitFor));
        //WaitUntil waitUntil = wait == true;
        m_MessageText.text = "Go!";
        //waitFor = 0.5f;
        //StartCoroutine(WaitCommand(waitFor));
        m_MessageText.gameObject.SetActive(false);
        Time.timeScale = 1;
>>>>>>> Stashed changes

    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScoreText()
    {
<<<<<<< Updated upstream
        m_ScoreDiplay.text = m_Score.ToString();
=======
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
        if(m_DisplayTime <= 0)
        {
            GameOver();
        }
    }
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
>>>>>>> Stashed changes
    }
    IEnumerator WaitCommand(float waitFor)
    {
        Debug.Log("Waiting for " + waitFor + " seconds");
        yield return new WaitForSecondsRealtime(waitFor);

        Debug.Log("Done waiting");
    }
}
