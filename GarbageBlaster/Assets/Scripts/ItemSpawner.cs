using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject m_ItemToSpawn;
    public Transform[] m_SpawnPoints;
    public float m_timer;
    private GameObject m_currentInstance;
    public int m_MaxGarbageCount;
    public int m_GarbageCount;
    public bool[] m_HasGarbage;
    public int randomIndex;
    // Start is called before the first frame update
    void Start()
    {
        m_MaxGarbageCount--;
        StartCoroutine(SpawnItem());
    }

    IEnumerator SpawnItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_timer);
            if (m_GarbageCount <= m_MaxGarbageCount)
            {
                randomIndex = Random.Range(0, m_SpawnPoints.Length);
                Debug.Log(randomIndex);
                if (m_HasGarbage[randomIndex] == false)
                {
                    Vector3 position = m_SpawnPoints[randomIndex].position;
                    Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    m_currentInstance = Instantiate(m_ItemToSpawn, position, rotation) as GameObject;
                    m_GarbageCount++;
                    m_HasGarbage[randomIndex] = true;
                    Debug.Log("Garbage Count = "+m_GarbageCount);
                }
                else
                {
                    Debug.Log("There is already Garbage here");
                    break;
                }
            }
            else
            {
                Debug.Log("Max Garbage Reached");
            }
        }
    }
}
