using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
    public InputField m_NameInput;
    public Component[] m_PlayerParts;
    public Text m_HighscoresNames;
    public Text m_HighscoresScores;
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
    public bool GameStateStartUp;
    //private bool wait = false;
    // Start is called before the first frame update
    public enum GameState
    {
        Opening,
        Start,
        Playing,
        GameOver,
        Paused
    };
    public GameState m_GameState;
    public GameState State { get { return m_GameState; } }
    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Directory - " + currentDirectory);
        m_SettingEditor.LoadSettings(m_VolumeSlider, m_FOVSlider);
        m_HighScoreEditor.LoadScores();
        GameStateStartUp = true;
        m_GameState = GameState.Start;
    }
    // Update is called once per frame
    void Update()
    {
        switch (m_GameState)
        {
            //case GameState.Opening:
                //break;
            case GameState.Start:
                while (GameStateStartUp == true)
                {
                    m_PlayerParts[0].gameObject.transform.position = new Vector3(0, -0.5f, 0);
                    m_PlayerParts[0].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    foreach (GameObject gameObject in m_InGameHUD)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    m_PauseMenu[4].SetActive(false);
                    foreach (GameObject gameObject in m_PauseMenu)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    m_PauseMenu[0].gameObject.SetActive(true);
                    m_PauseMenu[2].gameObject.SetActive(true);
                    Time.timeScale = 0;
                    m_isGamePaused = true;
                    foreach (Component part in m_PlayerParts)
                    {
                        part.gameObject.SetActive(false);
                    }
                    m_DisplayTime = m_LevelTime;
                    GameStateStartUp = false;
                }
                break;
            case GameState.Playing:
                while (GameStateStartUp == true)
                {
                    ChangeVolume();
                    ChangeFOV();
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Time.timeScale = 1;
                    m_Score = 0;
                    VacGun vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
                    foreach (GameObject gameObject in m_InGameHUD)
                    {
                        gameObject.gameObject.SetActive(true);
                    }
                    foreach (GameObject gameObject in m_PauseMenu)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    foreach (Component part in m_PlayerParts)
                    {
                        part.gameObject.SetActive(true);
                    }
                    //m_GameOver = false;
                    if (m_NameInput.text != "")
                    {
                        m_PlayerName = m_NameInput.text;
                    }
                    else
                    {
                        m_PlayerName = "John Doe";
                    }
                    vacGun.m_LoadedGarbage = 0;
                    m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
                    m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
                    m_MessageText.gameObject.SetActive(false);
                    AudioListener.volume = m_VolumeSlider.value;
                    m_VolumeSliderText.text = "Volume: " + Mathf.RoundToInt(m_VolumeSlider.value * 100).ToString() + "%";
                    m_FOVSliderText.text = "FOV: " + Mathf.RoundToInt((m_FOVSlider.value * 100) + 30).ToString();
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
                    GameStateStartUp = false;
                }
                if (Input.GetKeyUp(KeyCode.Escape) && m_isGamePaused == false)
                {
                    PauseGame();
                }
                UpdateTimer();
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                while (GameStateStartUp == true)
                {
                    ItemSpawner itemSpawner = UnityEngine.Object.FindObjectOfType<ItemSpawner>();
                    foreach (GameObject garbageball in itemSpawner.m_AllGarbage)
                    {
                        garbageball.SetActive(false);
                    }
                    itemSpawner.m_AllGarbage.RemoveRange(0, itemSpawner.m_AllGarbage.Count);
                    m_MessageText.gameObject.SetActive(true);
                    Debug.Log("Game Over");
                    PauseGame();
                    m_isGamePaused = false;
                    m_PauseMenu[1].SetActive(false);
                    m_PauseMenu[3].SetActive(true);
                    m_MessageText.text = "Time's Up!";
                    //waitFor = 2;
                    //StartCoroutine(WaitCommand(waitFor));
                    m_HighScoreEditor.AddScore(m_Score, m_PlayerName);
                    highscoreEditor highscoreEditor = UnityEngine.Object.FindObjectOfType<highscoreEditor>();
                    if (highscoreEditor.m_AddedToHighscores == true)
                    {
                        m_MessageText.text = "Your final score was " + m_Score + "!\nWell done!";
                    }
                    else
                    {
                        m_MessageText.text = "Your final score was " + m_Score + ".";
                    }
                    m_HighScoreEditor.SaveScores();
                    GameStateStartUp = false;
                }
                break;
        }
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
        m_HighScoreEditor.SaveScores();
        m_SettingEditor.SaveSettings(m_VolumeSlider, m_FOVSlider);
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
                gameObject.gameObject.SetActive(false);
            }
        m_PauseMenu[0].gameObject.SetActive(true);
        m_PauseMenu[1].gameObject.SetActive(true);
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
        foreach (Component part in m_PlayerParts)
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
            //while (m_GameOver == false)
            //{
            GameStateStartUp = true;
                m_GameState = GameState.GameOver;
                //m_GameOver = true;
            //}
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
    IEnumerator WaitCommand(float waitFor)
    {
        Debug.Log("Waiting for " + waitFor + " seconds");
        yield return new WaitForSecondsRealtime(waitFor);

        Debug.Log("Done waiting");

    }
    public void StartPlaying()
    {
        if (m_NameInput.text != "")
        {
            GameStateStartUp = true;
            m_isGamePaused = false;
            m_GameState = GameState.Playing;
        }
    }
    public void RestartGame()
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
        foreach (Component part in m_PlayerParts)
        {
            part.gameObject.SetActive(true);
        }
        m_PlayerParts[0].gameObject.transform.position = new Vector3(0, -0.5f, 0);
        m_PlayerParts[0].gameObject.transform.position = new Vector3(0, 0.4000001f, 0);
        m_Score = 0;
        m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
        VacGun vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
        vacGun.m_LoadedGarbage = 0;
        m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
        m_DisplayTime = m_LevelTime;
        m_GameState = GameState.Playing;
    }
    public void DisplayHighscores()
    {
        highscoreEditor highscoreEditor = UnityEngine.Object.FindObjectOfType<highscoreEditor>();
        foreach (GameObject gameObject in m_PauseMenu)
        {
            gameObject.gameObject.SetActive(false);
        }
        m_PauseMenu[0].SetActive(true);
        m_PauseMenu[4].SetActive(true);
        int i = 0;
        string HSNamesList = "";
        string HSScoresList = "";
        foreach (int score in highscoreEditor.m_Scores)
        {
            HSNamesList += highscoreEditor.m_ScoreNames[i] + "\n";
            HSScoresList += highscoreEditor.m_Scores[i].ToString() + "\n";
            i++;
        }
        m_HighscoresNames.text = HSNamesList;
        m_HighscoresScores.text = HSScoresList;
    }
    public void ReturnToStart()
    {
        GameStateStartUp = true;
        m_GameState = GameState.Start;
    }
}
