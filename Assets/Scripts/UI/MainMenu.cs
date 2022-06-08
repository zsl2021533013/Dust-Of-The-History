using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    public GameObject mainMenu;

    [SerializeField]
    public GameObject languageMenu;

    [SerializeField]
    AudioClip m_mainMenuBGM;

    [SerializeField]
    AudioClip m_MenuClickSound;

    [SerializeField]
    AudioClip m_menu;

    Button newGameBtn;

    Button continueBtn;

    Button languageBtn;

    Button quitBtn;

    PlayableDirector director;

    private void Awake()
    {
        newGameBtn = transform.GetChild(1).GetComponent<Button>();
        continueBtn = transform.GetChild(2).GetComponent<Button>();
        languageBtn = transform.GetChild(3).GetComponent<Button>();
        quitBtn = transform.GetChild(4).GetComponent<Button>();

        director = FindObjectOfType<PlayableDirector>();
    }

    void Start()
    {
        newGameBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueBtn);
        quitBtn.onClick.AddListener(QuitGameBtn);
        languageBtn.onClick.AddListener(LanguageBtn);

        director.stopped += NewGameBtn; // 开场动画播出后开始加载游戏

        BGMManager.Instance.PlayBGM(m_mainMenuBGM);
    }

    void Update()
    {
        
    }

    void PlayTimeLine()
    {
        BGMManager.Instance.PlayOneShot(m_MenuClickSound);
        director.Play();
    }

    void NewGameBtn(PlayableDirector playableDirector)
    {
        InventoryManager.Instance.leaveScene0();
        PlayerPrefs.DeleteAll();
        TranstionManager.Instance.StartGame();
    }

    void ContinueBtn()
    {
        BGMManager.Instance.PlayOneShot(m_MenuClickSound);
        InventoryManager.Instance.leaveScene0();
        if (!PlayerPrefs.HasKey("CurrentSceneName")) return;
        TranstionManager.Instance.TransitionToScene(TranstionManager.Instance.CurrentSceneName);
    }

    void LanguageBtn()
    {
        BGMManager.Instance.PlayOneShot(m_menu);
        languageMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    void QuitGameBtn()
    {
        BGMManager.Instance.PlayOneShot(m_menu);
        Application.Quit();
    }
}
