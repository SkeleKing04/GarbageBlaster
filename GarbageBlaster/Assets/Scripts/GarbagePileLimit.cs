using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbagePileLimit : MonoBehaviour
{
    public int m_MaxGarbageLimit;
    public int m_GarbagePileAmount;
    public bool m_GarbagePileFull;
    public GameObject[] m_GarbagePileStates;
    public string m_SpawnerNum;
    // Start is called before the first frame update
    void Start()
    {
        m_GarbagePileAmount = 0;
        m_GarbagePileFull = false;
        m_SpawnerNum = name;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GarbagePileAmount >= m_MaxGarbageLimit)
        {
            m_GarbagePileFull = true;
        }
    }
}
