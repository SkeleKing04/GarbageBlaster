using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public GameObject m_Collectable;
    public int m_SpawnLocal;
    public AudioClip m_garbageNoise;
    // Start is called before the first frame update
    void Start()
    {
        ItemSpawner itemSpawner = Object.FindObjectOfType<ItemSpawner>();
        m_SpawnLocal = itemSpawner.randomIndex;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(m_garbageNoise, gameObject.transform.position);
            ItemSpawner itemSpawner = Object.FindObjectOfType<ItemSpawner>();
            itemSpawner.m_GarbageCount--;
            itemSpawner.m_SpawnPointFull[m_SpawnLocal] = false;
            VacGun vacGun = Object.FindObjectOfType<VacGun>();
            vacGun.m_LoadedGarbage++;
            GameManager gameManager = Object.FindObjectOfType<GameManager>();
            gameManager.UpdateGarbageText();
            Destroy(gameObject);
            Debug.Log(itemSpawner.m_GarbageCount);
        }
    }
}
