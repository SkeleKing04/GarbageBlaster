using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigation : MonoBehaviour
{
    private NavMeshAgent m_NavAgent;
    private Rigidbody m_Rigidbody;
    public Transform[] m_nodes;
    private bool m_Navigating;
    private int m_currentNode;
    private void Awake()
    {
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
    }
    private void OnDisable()
    {
        m_Rigidbody.isKinematic = true;
    }
    private void Start()
    {
        goToNode();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void goToNode()
    {
        m_currentNode = 0;
        foreach (Transform transform in m_nodes)
        {
            //m_Navigating = true;
            //while (m_Navigating == true)
            //{
            Debug.Log("Heading to " + m_nodes[m_currentNode]);
            //m_NavAgent.SetDestination(m_nodes[i].transform.position);
            //switch (m_NavAgent.isStopped)
            //{
            //case true:
            //m_Navigating = false;
            //break;
            //case false:
            //break;
            //}
            //}
            m_currentNode++;

        }
        goToNode();
    }
}
