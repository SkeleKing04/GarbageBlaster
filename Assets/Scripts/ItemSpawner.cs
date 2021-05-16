using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject m_ItemToSpawn;
    public Transform[] m_SpawnPoints;
    public float m_timer;
    public GameObject m_currentInstance;
    public int m_MaxGarbageCount;
    public int m_GarbageCount;
    public bool[] m_SpawnPointFull;
    public bool m_garbageSpawned;
    public int randomIndex;
    public List<GameObject> m_AllGarbage = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItem());
    }
    void Update()
    {

    }

    IEnumerator SpawnItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(m_timer);
            if (m_GarbageCount <= m_MaxGarbageCount)
            {
                m_garbageSpawned = false;
                while (m_garbageSpawned == false)
                {
                    randomIndex = Random.Range(0, m_SpawnPoints.Length);
                    if (m_SpawnPointFull[randomIndex] == false)
                    {
                        Vector3 position = m_SpawnPoints[randomIndex].position;
                        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        m_currentInstance = Instantiate(m_ItemToSpawn, position, rotation) as GameObject;
                        m_GarbageCount++;
                        m_SpawnPointFull[randomIndex] = true;
                        m_garbageSpawned = true;
                        GameManager gameManager = Object.FindObjectOfType<GameManager>();
                        m_AllGarbage.Add(m_currentInstance);
                        Debug.Log(m_GarbageCount);
                    }
                    else
                    {
                        Debug.Log("There is already garbage here");
                        m_garbageSpawned = false;
                    }
                }
            }
            else
            {
                Debug.Log("Max Garbage Reached");
            }
        }
    }
}
