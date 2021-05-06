using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class settingEditor : MonoBehaviour
{
    string currentDirectory;
    public string m_SettingsFileName = "settings.txt";
    public float[] m_SettingsValues = new float[2];
    // Start is called before the first frame update
    void Start()
    {
        currentDirectory = Application.dataPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadSettings(Slider m_VolumeSlider, Slider m_FOVSlider)
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
    public void SaveSettings(Slider m_VolumeSlider, Slider m_FOVSlider)
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
}
