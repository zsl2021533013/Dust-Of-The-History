using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Scene2BossCGController : MonoBehaviour
{
    [SerializeField]
    PlayableDirector m_playableDirector;

    private bool m_start = false;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !m_start)
        {
            m_playableDirector.Play();
            m_start = true;
        }
    }
}
