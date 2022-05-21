using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocData : MonoBehaviour, ILanguageObserver
{
    [SerializeField]
    string m_key;

    TextMeshProUGUI m_tmp;

    // Start is called before the first frame update
    void Start()
    {
        m_tmp = GetComponent<TextMeshProUGUI>();

        string value = LocalizationManager.Instance.GetLocalizationValue(m_key);

        m_tmp.text = value;

        LocalizationManager.Instance.RegisterLanguageObserver(this);
    }

    public void LoadLanguage()
    {
        string value = LocalizationManager.Instance.GetLocalizationValue(m_key);

        m_tmp.text = value;

        Debug.Log(value);
    }
}
