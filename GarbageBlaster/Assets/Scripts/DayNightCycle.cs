using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public GameObject m_CenterPoint;
    public int m_RotateAmount = 1;
    // Start is called before the first frame update
    void Start()
    {
        IEnumerator;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Penis");
        m_CenterPoint.transform.rotation = new Quaternion(m_RotateAmount, 0, 0, 1);
    }
    IEnumerator
    {
        m_CenterPoint.transform.rotation = new Quaternion(m_RotateAmount, 0, 0, 1);

    }
}
