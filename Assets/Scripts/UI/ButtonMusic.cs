using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonMusic : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    public AudioClip m_clip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        BGMManager.Instance.PlayOneShotWithPitch(m_clip, 1.5f, 0.2f);
    }
}
