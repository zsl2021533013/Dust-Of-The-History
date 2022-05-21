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

        director.stopped += NewGameBtn; // ��������������ʼ������Ϸ
    }

    void Update()
    {
        
    }

    void PlayTimeLine()
    {
        director.Play();
    }

    void NewGameBtn(PlayableDirector playableDirector)
    {
        PlayerPrefs.DeleteAll();
        TranstionManager.Instance.StartGame();
    }

    void ContinueBtn()
    {
        if (!PlayerPrefs.HasKey("CurrentSceneName")) return;
        TranstionManager.Instance.TransitionToScene(TranstionManager.Instance.CurrentSceneName);
    }

    void LanguageBtn()
    {
        languageMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    void QuitGameBtn()
    {
        Application.Quit();
    }
}
