using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
        VacGun vacGun = Object.FindObjectOfType<VacGun>();
        m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
        foreach (GameObject gameObject in m_PauseMenu)
        {
            gameObject.gameObject.SetActive(false);
        }
        AudioListener.volume = m_VolumeSlider.value;
        m_VolumeSliderText.text = "Volume: " + Mathf.RoundToInt(m_VolumeSlider.value * 100).ToString() + "%";
        m_FOVSliderText.text = "FOV: " + Mathf.RoundToInt((m_FOVSlider.value * 100) + 30).ToString();
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
    }
    public void UpdateScoreText()
    {
        m_ScoreDisplay.text = "Score:\n"+m_Score.ToString();
    }
    public void UpdateGarbageText()
    {
        VacGun vacGun = Object.FindObjectOfType<VacGun>();
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
}
