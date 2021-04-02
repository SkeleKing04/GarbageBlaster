using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int m_Score;
    public Text m_ScoreDiplay;
    // Start is called before the first frame update
    void Start()
    {
        m_ScoreDiplay.text = 0.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScoreText()
    {
        m_ScoreDiplay.text = m_Score.ToString();
    }
}
