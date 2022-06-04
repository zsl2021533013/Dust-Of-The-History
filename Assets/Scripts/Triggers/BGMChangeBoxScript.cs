using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMChangeBoxScript : MonoBehaviour
{
    [SerializeField]
    AudioClip m_audioclip;
    
    private bool is_Boom = false;

    //change bgm 
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && is_Boom == false)
        {
            BGMManager.Instance.PlayBGM(m_audioclip);
            is_Boom = true;
        }
    }
}
