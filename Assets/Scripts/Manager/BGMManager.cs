using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : Singleton<BGMManager>
{
    [SerializeField] 
    private AudioSource m_BGMAudioSource;
    [SerializeField] 
    private AudioSource m_audioSource;
    private AudioClip m_nowClip = null;
    private AudioClip m_nextClip = null;

    private float m_cutTime = 0.0f;

    [SerializeField]
    private float CUTTIME = 2.0f;

    [SerializeField]
    float MAXVOLUME = 0.4f;
    [SerializeField]
    float MINVOLUME = 0.0f;

    private float v;

    private bool m_cutting = false;
    private bool m_fadingin = false;
    private bool cut = false;

    public void PlayBGM(AudioClip clip)
    {
        m_BGMAudioSource.loop = false;
        if (!m_nowClip)
        {
            //if not exist clip then initialize
            m_nowClip = clip;
            m_nextClip = clip;

            m_cutting = true;
            m_fadingin = false ;
            cut = false;
            m_BGMAudioSource.volume = MINVOLUME;
        }
        else
        {
            if (m_nowClip == clip) return;
            m_nextClip = clip;//set the next clip
            //start change BGM
            m_cutting = true;
            m_fadingin = true;
            cut = false;
        }
    }

    public void PlayOneShot(AudioClip clip)
    {
        m_audioSource.volume = MAXVOLUME;
        m_audioSource.loop = false;
        m_audioSource.PlayOneShot(clip);
    }

    //first volume changed to min
    //then change BGM clip
    //volume changed to max
    private void Cutting()
    {
        if (m_fadingin)//volume to min
        {
            m_BGMAudioSource.volume = Mathf.SmoothDamp(m_BGMAudioSource.volume, MINVOLUME, ref v, CUTTIME);
            if(m_BGMAudioSource.volume < MINVOLUME + 0.01f)
            {
                m_fadingin = false;
            }
        }
        else//volume to max
        {
            if (!cut)
            {
                m_BGMAudioSource.clip = m_nextClip;
                m_BGMAudioSource.Play();
                cut = true;
            }
            m_BGMAudioSource.volume = Mathf.SmoothDamp(m_BGMAudioSource.volume, MAXVOLUME, ref v, CUTTIME);
            if (m_BGMAudioSource.volume > MAXVOLUME - 0.01f)
            {
                m_cutting = false;
                m_nowClip = m_nextClip;
                m_nextClip = null;
            }
        }
    }

    private void Update()
    {
        if (m_cutting) Cutting();//if changing,then run it!
    }
}
