using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScoredCheck : MonoBehaviour
{
    public Collider m_itemContact;
    public GameObject m_Garbage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Garbage")
        {
            Debug.Log("Garbage collected!");
            Destroy(collision.gameObject);
            ItemSpawner itemSpawner = Object.FindObjectOfType<ItemSpawner>();
            itemSpawner.m_GarbageCount--;
            GameManager gameManager = Object.FindObjectOfType<GameManager>();
            gameManager.m_Score++;
        }
    }
}
