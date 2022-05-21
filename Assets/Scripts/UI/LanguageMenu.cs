using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour
{
    public GameObject mainMenu;

    public GameObject languageMenu;

    Button englishBtn;

    Button chineseBtn;

    private void Awake()
    {
        englishBtn = transform.GetChild(0).GetComponent<Button>();

        chineseBtn = transform.GetChild(1).GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        englishBtn.onClick.AddListener(EnglishBtn);
        chineseBtn.onClick.AddListener(ChineseBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnglishBtn()
    {
        LocalizationManager.Instance.m_currentLanguage = Language.English;
        mainMenu.SetActive(true);
        LocalizationManager.Instance.ChangeLanguage();
        languageMenu.SetActive(false);
    }

    void ChineseBtn()
    {
        LocalizationManager.Instance.m_currentLanguage = Language.Chinese;
        mainMenu.SetActive(true);
        LocalizationManager.Instance.ChangeLanguage();
        languageMenu.SetActive(false);
    }
}
