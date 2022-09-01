using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class TranstionManager : Singleton<TranstionManager>, IEndGameObserver
{
    public string CurrentSceneName { get { return PlayerPrefs.GetString("CurrentSceneName"); } }

    private GameObject player;

    private NavMeshAgent agent;

    private bool gameEnd = true;

    [SerializeField]
    AudioClip m_GameBgm;
    [SerializeField]
    AudioClip m_MainMenuBgm;

    private void Start()
    {
        GameManager.Instance.RigisterObserver(this);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            TransitionToMain();
        }
    }

    public void RigisterPlayer(GameObject m_player)
    {
        Debug.Log("TranstionManager has rigister player");
        player = m_player;
        agent = player.GetComponent<NavMeshAgent>();
    }

    public void TransitionToDestination(PortalController portalController)
    {
        switch (portalController.transitionType)
        {
            case TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, portalController.destinationTag));
                break;
            case TransitionType.DifferentScene:
                StartCoroutine(Transition(portalController.sceneName, portalController.destinationTag));
                break;
        }
    }

    public void TransitionToScene(string scene)
    {
        if (scene == null) return;
        StartCoroutine(LoadScene(scene));
    }

    public void TransitionToMain()
    {
        StartCoroutine(LoadMainMenu());
    }

    public void StartGame()
    {
        StartCoroutine(LoadTheInitializedGame());
    }

    IEnumerator Transition(string sceneName, DestinationTag destinationTag)
    {
        SaveManager.Instance.SavePlayerData();
        if(sceneName != SceneManager.GetActiveScene().name) // 此时为不同场景转换
        {
            FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = true;
            yield return StartCoroutine(FadeCanvasManager.Instance.FadeOut(FadeCanvasManager.Instance.fadeOutTime));

            yield return SceneManager.LoadSceneAsync(sceneName);

            Transform target = FindDetinationTag(destinationTag);
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.SetPositionAndRotation(target.position, target.rotation);
            player.GetComponent<NavMeshAgent>().enabled = true;

            SaveManager.Instance.LoadPlayerData();

            yield return StartCoroutine(FadeCanvasManager.Instance.FadeIn(FadeCanvasManager.Instance.fadeInTime));
            FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = false;

            yield break;
        }
        else
        {
            Transform target = FindDetinationTag(destinationTag);

            agent.enabled = false;

            player.transform.SetPositionAndRotation(target.position, target.rotation);

            agent.enabled = true;

            yield return null;  
        }
    }

    Transform FindDetinationTag(DestinationTag destinationTag)
    {
        var targets = FindObjectsOfType<TransitionDestination>();
        foreach (var target in targets)
        {
            if (target.destinationTag == destinationTag)
            {
                return target.transform;
            }
        }
        return null;    
    }

    void OpenInventory()
    {
        InventoryManager.Instance.transform.GetChild(0).gameObject.SetActive(true);
        InventoryManager.Instance.transform.GetChild(1).gameObject.SetActive(false);
        InventoryManager.Instance.transform.GetChild(2).gameObject.SetActive(false);
        InventoryManager.Instance.transform.GetChild(3).gameObject.SetActive(true);
        //TODO: 如此的话，那么可以在标题界面打开背包，需要修改
    }

    IEnumerator LoadScene(string sceneName) // 直接加载第几场景
    {
        if(name != "")
        {
            FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = true;
            yield return StartCoroutine(FadeCanvasManager.Instance.FadeOut(FadeCanvasManager.Instance.fadeOutTime));

            yield return SceneManager.LoadSceneAsync(sceneName);

            SaveManager.Instance.LoadPlayerData();

            yield return StartCoroutine(FadeCanvasManager.Instance.FadeIn(FadeCanvasManager.Instance.fadeInTime));
            FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = false;

            BGMManager.instance.PlayBGM(m_GameBgm);

            yield break;
        }
    }

    IEnumerator LoadTheInitializedGame() // 开始游戏，加载第一场景
    {
        FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = true;
        yield return StartCoroutine(FadeCanvasManager.Instance.FadeOut(FadeCanvasManager.Instance.fadeOutTime));

        yield return Transition("Scene 1", DestinationTag.Scene_1_Portal_1); // 加载场景

        Debug.Log("Scene 1 has loaded");

        OpenInventory();

        SaveManager.Instance.LoadPlayerData(); // 加载人物数据

        Debug.Log("Player's data has loaded");

        PlayerController.Instance.characterStats.characterData = Instantiate(PlayerController.Instance.characterStats.templateCharacterData); // 数值重置

        yield return StartCoroutine(FadeCanvasManager.Instance.FadeIn(FadeCanvasManager.Instance.fadeInTime));
        FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = false;

        BGMManager.instance.PlayBGM(m_GameBgm);

        yield break;
        
    }

    IEnumerator LoadMainMenu() // 加载主场景
    {
        FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = true;
        yield return StartCoroutine(FadeCanvasManager.Instance.FadeOut(FadeCanvasManager.Instance.fadeOutTime));

        yield return SceneManager.LoadSceneAsync("Scene 0");

        yield return StartCoroutine(FadeCanvasManager.Instance.FadeIn(FadeCanvasManager.Instance.fadeInTime));
        FadeCanvasManager.Instance.gameObject.GetComponent<Canvas>().enabled = false;

        BGMManager.instance.PlayBGM(m_MainMenuBgm);

        yield break;
    }

    void IEndGameObserver.EndNotify()
    {
        if (gameEnd)
        {
            gameEnd = false;
            StartCoroutine(LoadMainMenu());
        }
    }
}
