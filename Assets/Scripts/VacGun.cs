using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VacGun : MonoBehaviour
{
    public Rigidbody m_GarbageBag;
    public Transform m_FireTransform;
    public float m_LaunchForce = 30f;
    public int m_LoadedGarbage = 0;
    public Transform m_RotateAxis;

    // Update is called once per frame
    void Update()
    {
        if (m_LoadedGarbage >= 1)
        {
            if (Input.GetMouseButtonUp(0))
            {
                Fire();
            }
        }
    }
    private void Fire()
    {
        Rigidbody GarbageInstance = Instantiate(m_GarbageBag, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
        GarbageInstance.velocity = m_LaunchForce * m_FireTransform.forward;
        m_LoadedGarbage--;
        ItemSpawner itemSpawner = Object.FindObjectOfType<ItemSpawner>();
        itemSpawner.m_GarbageCount++;
        itemSpawner.m_AllGarbage.Add(GarbageInstance.gameObject);
        Debug.Log(itemSpawner.m_GarbageCount);
    }
}
