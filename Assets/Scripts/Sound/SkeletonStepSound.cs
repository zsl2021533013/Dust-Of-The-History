using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonStepSound : MonoBehaviour
{
    [SerializeField]
    NavMeshAgent m_agent;

    [SerializeField]
    AudioSource m_audioSource;

    [SerializeField]
    AudioClip m_walk;

    private float m_walkTime = 0.3f;
    float m_walkTimeNow = 0;

    private void Update()
    {
        if(m_agent.velocity.sqrMagnitude > 0.3f)
        {
            if (m_walkTimeNow > m_walkTime)
            {
                m_audioSource.PlayOneShot(m_walk);
                m_walkTimeNow = 0;
            }
            else
            {
                m_walkTimeNow += Time.deltaTime;
            }
        }
    }
    public void FootL()
    {

    }
    
    public void FootR()
    {

    }
}
