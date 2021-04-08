using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_Score = 0;
    public Text m_ScoreDisplay;
    public Text m_GarbageLoadedDisplay;
    // Start is called before the first frame update
    void Start()
    {
        m_ScoreDisplay.text = "Score:\n" + m_Score.ToString();
        VacGun vacGun = Object.FindObjectOfType<VacGun>();
        m_GarbageLoadedDisplay.text = "Ammo:\n" + vacGun.m_LoadedGarbage.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
