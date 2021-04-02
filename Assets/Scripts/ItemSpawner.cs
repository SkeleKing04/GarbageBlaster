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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItem());
    }

    IEnumerator SpawnItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_timer);
            if (m_GarbageCount <= m_MaxGarbageCount)
            {
                int randomIndex = Random.Range(0, m_SpawnPoints.Length);
                Vector3 position = m_SpawnPoints[randomIndex].position;
                Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                m_currentInstance = Instantiate(m_ItemToSpawn, position, rotation) as GameObject;
                m_GarbageCount++;
                Debug.Log(m_GarbageCount);
            }
            else
            {
                Debug.Log("Max Garbage Reached");
            }
        }
    }
}
