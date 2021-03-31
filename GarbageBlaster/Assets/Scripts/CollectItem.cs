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
            ItemSpawner garbageCount = Object.FindObjectOfType<ItemSpawner>();
            garbageCount.m_GarbageCount -= 1;
            Destroy(gameObject);
            Debug.Log(garbageCount.m_GarbageCount);
        }
    }
}
