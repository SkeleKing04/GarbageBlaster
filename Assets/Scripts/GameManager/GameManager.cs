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
    public bool UpdatePauseState = false;
    //private bool wait = false;
    // Start is called before the first frame update
    public enum GameState
    {
        Opening,
        Start,
        Playing,
        GameOver,
        Paused,
        Quiting,
        Waiting
    };
    public enum PauseState
    {
        Off,
        StartMenu,
        PauseMenu,
        GameOverMenu,
        HighscoreMenu,
        Waiting
    };
    public GameState m_GameState;
    public PauseState m_PauseState;
    public GameState StateGame { get { return m_GameState; } }
    public PauseState StatePause { get { return m_PauseState; } }
    void Start()
    {
        currentDirectory = Application.dataPath;
        Debug.Log("Directory - " + currentDirectory);
        m_SettingEditor.LoadSettings(m_VolumeSlider, m_FOVSlider);
        m_HighScoreEditor.LoadScores();
        m_GameState = GameState.Opening;
    }
    // Update is called once per frame
    void Update()
    {
        switch (m_GameState)
        {
            //case GameState.Opening:
            //break;
            case GameState.Waiting:
                break;
            case GameState.Opening:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                UpdatePauseState = true;
                m_PauseState = PauseState.StartMenu;
                m_GameState = GameState.Paused;
                break;
            case GameState.Start:
                ChangeVolume();
                ChangeFOV();
                m_PlayerParts[0].gameObject.transform.position = new Vector3(0, -0.5f, 0);
                m_PlayerParts[0].gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                ItemSpawner itemSpawner = UnityEngine.Object.FindObjectOfType<ItemSpawner>();
                int i = 0;
                foreach (bool bools in itemSpawner.m_SpawnPointFull)
                {
                    itemSpawner.m_SpawnPointFull[i] = false;
                    i++;
                }
                itemSpawner.m_GarbageCount = 0;
                itemSpawner.m_garbageSpawned = false;
                m_DisplayTime = m_LevelTime;
                UpdatePauseState = true;
                m_PauseState = PauseState.Off;
                m_GameState = GameState.Playing;
                break;
            case GameState.Playing:
                    //MOVE    AudioListener.volume = m_VolumeSlider.value;
                    //m_VolumeSliderText.text = "Volume: " + Mathf.RoundToInt(m_VolumeSlider.value * 100).ToString() + "%";
                    //m_FOVSliderText.text = "FOV: " + Mathf.RoundToInt((m_FOVSlider.value * 100) + 30).ToString();
                    //m_MessageText.text = "Ready...";
                    //Time.timeScale = 0;
                    //waitFor = 3;
                    //StartCoroutine(WaitCommand(waitFor));
                    //WaitUntil waitUntil = wait == true;
                    //m_MessageText.text = "Go!";
                    //StartCoroutine(Timer(m_DisplayTime, m_TimerDisplay));
                    //waitFor = 0.5f;
                    //StartCoroutine(WaitCommand(waitFor));
                    //Time.timeScale = 1;
                if (Input.GetKeyUp(KeyCode.Escape) && m_isGamePaused == false)
                {
                    PauseGame();
                }
                m_DisplayTime -= Time.deltaTime;
                int seconds = Mathf.RoundToInt(m_DisplayTime);
                m_TimerDisplay.text = string.Format("{0:D2}:{1:D2}", (seconds / 60), (seconds % 60));
                if (m_DisplayTime <= 0)
                {
                    //vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
                    //vacGun.m_LoadedGarbage = 0;
                    //m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
                    GameStateStartUp = true;
                    m_GameState = GameState.GameOver;
                }
                m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
                VacGun vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
                m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
                break;
            case GameState.Paused:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                foreach (GameObject gameObject in m_InGameHUD)
                {
                    gameObject.gameObject.SetActive(false);
                }
                Time.timeScale = 0;
                m_isGamePaused = true;
                foreach (Component part in m_PlayerParts)
                {
                    part.gameObject.SetActive(false);
                }
                break;
            case GameState.GameOver:
                m_Score = 0;
                vacGun = UnityEngine.Object.FindObjectOfType<VacGun>();
                vacGun.m_LoadedGarbage = 0;
                m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
                m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
                itemSpawner = UnityEngine.Object.FindObjectOfType<ItemSpawner>();
                foreach (GameObject garbageball in itemSpawner.m_AllGarbage)
                {
                    garbageball.SetActive(false);
                }
                itemSpawner.m_AllGarbage.RemoveRange(0, itemSpawner.m_AllGarbage.Count);
                Debug.Log("Game Over");
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
                m_GameState = GameState.Paused;
                UpdatePauseState = true;
                m_PauseState = PauseState.GameOverMenu;
                break;
            case GameState.Quiting:
                m_HighScoreEditor.SaveScores();
                m_SettingEditor.SaveSettings(m_VolumeSlider, m_FOVSlider);
                Debug.Log("Quiting... Loser!");
                Application.Quit();
                break;
        }
        switch (m_PauseState)
        {
            case PauseState.Off:
                while (UpdatePauseState == true)
                {
                    foreach (GameObject gameObject in m_PauseMenu)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    foreach (GameObject gameObject in m_InGameHUD)
                    {
                        gameObject.gameObject.SetActive(true);
                    }
                    Time.timeScale = 1;
                    m_isGamePaused = false;
                    foreach (Component part in m_PlayerParts)
                    {
                        part.gameObject.SetActive(true);
                    }
                    UpdatePauseState = false;
                }
                break;
            case PauseState.StartMenu:
                while (UpdatePauseState == true)
                {

                    foreach (GameObject gameObject in m_PauseMenu)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    m_PauseMenu[0].SetActive(true);
                    m_PauseMenu[2].SetActive(true);
                    UpdatePauseState = false;
                }
                break;
            case PauseState.PauseMenu:
                while (UpdatePauseState == true)
                {
                    foreach (GameObject gameObject in m_PauseMenu)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    m_PauseMenu[0].SetActive(true);
                    m_PauseMenu[1].SetActive(true);
                    UpdatePauseState = false;
                }
                break;
            case PauseState.GameOverMenu:
                while (UpdatePauseState == true)
                {
                    foreach (GameObject gameObject in m_PauseMenu)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    m_PauseMenu[0].SetActive(true);
                    m_PauseMenu[3].SetActive(true);
                    UpdatePauseState = false;
                }
                break;
            case PauseState.HighscoreMenu:
                while (UpdatePauseState == true)
                {
                    foreach (GameObject gameObject in m_PauseMenu)
                    {
                        gameObject.gameObject.SetActive(false);
                    }
                    m_PauseMenu[0].SetActive(true);
                    m_PauseMenu[4].SetActive(true);
                    highscoreEditor highscoreEditor = UnityEngine.Object.FindObjectOfType<highscoreEditor>();
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
                    UpdatePauseState = false;
                }
                break;
        }
    }
    public void UpdateScoreText()
    {

    }
    public void UpdateGarbageText()
    {

    }
    public void QuitGame()
    {
        m_GameState = GameState.Quiting;
    }
    public void PauseGame()
    {
        m_GameState = GameState.Paused;
        UpdatePauseState = true;
        m_PauseState = PauseState.PauseMenu;
        /*Cursor.lockState = CursorLockMode.None;
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
        }*/
    }
    public void ResumeGame()
    {
        m_SettingEditor.SaveSettings(m_VolumeSlider, m_FOVSlider);
        UpdatePauseState = true;
        m_PauseState = PauseState.Off;
        m_GameState = GameState.Playing;

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
            UpdatePauseState = true;
            m_PauseState = PauseState.Off;
            GameStateStartUp = true;
            m_GameState = GameState.Start;
        }
    }
    public void RestartGame()
    {
        m_GameState = GameState.Start;
        UpdatePauseState = true;
        m_PauseState = PauseState.Off;
        /*Cursor.lockState = CursorLockMode.Locked;
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
        ItemSpawner itemSpawner = UnityEngine.Object.FindObjectOfType<ItemSpawner>();
        int i = 0;
        foreach (bool bools in itemSpawner.m_SpawnPointFull)
        {
            itemSpawner.m_SpawnPointFull[i] = false;
            i++;
        }
        itemSpawner.m_GarbageCount = 0;
        itemSpawner.m_garbageSpawned = false;
        m_DisplayTime = m_LevelTime;
        m_GameState = GameState.Playing;*/
    }
    public void DisplayHighscores()
    {
        UpdatePauseState = true;
        m_PauseState = PauseState.HighscoreMenu;
        /*foreach (GameObject gameObject in m_PauseMenu)
        {
            gameObject.gameObject.SetActive(false);
        }
        m_PauseMenu[0].SetActive(true);
        m_PauseMenu[4].SetActive(true);*/
    }
    public void ReturnToStart()
    {
        /*m_Score = 0;
        m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
        m_DisplayTime = m_LevelTime;
        GameStateStartUp = true;*/
        UpdatePauseState = true;
        m_PauseState = PauseState.StartMenu;
    }
}
