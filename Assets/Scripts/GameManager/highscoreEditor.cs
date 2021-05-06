using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class highscoreEditor : MonoBehaviour
{
    string currentDirectory;
    public string m_HighScoresFileName = "highscores.txt";
    public string m_HighScoreNames = "highscorenames.txt";
    public int[] m_Scores = new int[5];
    public string[] m_ScoreNames = new string[5];
    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;

    }

    // Update is called once per frame
    void Update()
    {
        
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
    public void AddScore(int m_Score, string m_PlayerName)
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
