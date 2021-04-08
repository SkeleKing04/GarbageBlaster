using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    public GameObject m_Collectable;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ItemSpawner itemSpawner = Object.FindObjectOfType<ItemSpawner>();
            itemSpawner.m_GarbageCount--;
            VacGun vacGun = Object.FindObjectOfType<VacGun>();
            vacGun.m_LoadedGarbage++;
            GameManager gameManager = Object.FindObjectOfType<GameManager>();
            gameManager.UpdateGarbageText();
            Destroy(gameObject);
            Debug.Log(itemSpawner.m_GarbageCount);
        }
    }
}
