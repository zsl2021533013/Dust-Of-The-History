using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Scene2BossCGController : MonoBehaviour
{
    [SerializeField]
    PlayableDirector m_playableDirector;

    [SerializeField]
    CameraManager m_cameraManager;

    private bool m_start = false;

    private bool delay_finished = false;

    private float DELAYTIME = 2.0f;
    private float delaytime = 0;

    private bool end = false;

    public void Play()
    {
        if (!m_start && !end)
        {
            m_cameraManager.BroadcastCG(0,6.0f,1.0f);
            m_start = true;
        }
    }

    private void M_playableDirector_stopped(PlayableDirector obj)
    {
        m_cameraManager.EndCG(4.0f,1.0f);
    }

    private void Update()
    {
        if (m_start && !end)
        {
            delaytime += Time.deltaTime;
            if (delaytime > DELAYTIME)
            {
                m_playableDirector.Play();
                end = true;
                m_playableDirector.stopped += M_playableDirector_stopped;
            }
        }
    }
}
