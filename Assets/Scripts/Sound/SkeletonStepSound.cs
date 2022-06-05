using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStepSound : MonoBehaviour
{
    [SerializeField]
    EnemyController m_enemyController;

    [SerializeField]
    AudioSource m_audioSource;

    [SerializeField]
    AudioClip m_walk;

    private float m_walkTime = 0.3f;
    float m_walkTimeNow = 0;

    private void Update()
    {
        if(m_enemyController.enemyState == EnemyState.CHASE)
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
