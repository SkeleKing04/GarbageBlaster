using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public int CycleSpeed;
    public GameObject m_OrbitCenter;
    private int i = 0;
    // Update is called once per frame
    void FixedUpdate()
    {
        m_OrbitCenter.transform.rotation = new Quaternion(i, 0f, 0f, 0f);
        i++;
    }
}
