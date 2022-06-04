using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    Cinemachine.CinemachineImpulseSource m_CinemachineImpulseSource;

    [SerializeField]
    float ShakeTime = 4.5f;

    [SerializeField]
    GameObject myself;

    float shake_time_now = 0;

    // Update is called once per frame
    void Update()
    {
        if (shake_time_now < ShakeTime)
        {
            m_CinemachineImpulseSource.GenerateImpulse();
            shake_time_now += Time.deltaTime;
        }
        else
        {
            myself.SetActive(false);
        }
    }
}
