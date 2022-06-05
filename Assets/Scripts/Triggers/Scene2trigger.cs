using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene2trigger : MonoBehaviour
{
    private bool isActive = false;
    private float rotateTime = 1.5f;
    private float m_time = 0;

    private bool isUsed = false;

    [SerializeField]
    Scene2BossCGController Scene2BossCGController;

    [SerializeField]
    AudioClip audioClip;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && MouseManager.Instance.isClickTrigger && !isActive && !isUsed)
        {
            isActive = true;
            isUsed = true;
            tag = "Ground";
            BGMManager.Instance.PlayBGM(audioClip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Rotate(new Vector3(0, 1, 0));
            m_time += Time.deltaTime;
            if (m_time > rotateTime)
            {
                isActive = false;
                Scene2BossCGController.Play();
            }
        }
    }
}
