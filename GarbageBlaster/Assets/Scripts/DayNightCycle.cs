using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public GameObject m_CenterPoint;
    public int m_RotateAmount = 1;
    private int rotateIncrease;
    // Start is called before the first frame update
    void Start()
    {
        //m_RotateAmount = m_RotateAmount * -1;
        rotateIncrease = m_RotateAmount;
    }
    // Update is called once per frame
    void FixedUpdate()
        {
            Debug.Log(rotateIncrease);
            m_CenterPoint.transform.rotation = new Quaternion(Time.deltaTime, 0, 0, rotateIncrease);
            rotateIncrease -= m_RotateAmount;
        }
}
