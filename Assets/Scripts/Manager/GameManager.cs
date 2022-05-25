using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats characterStats;

    public GameObject player;

    public bool isDemoDead = false;

    public bool isMachineGolemDead = false;

    public bool isRedDemoDead = false;

    private CinemachineFreeLook freeLookCamera;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();

    public void RigisterPlayer(GameObject m_player)
    {
        Debug.Log("GameManager has rigister player");
        player = m_player;
        characterStats = player.GetComponent<CharacterStats>();

        freeLookCamera = FindObjectOfType<CinemachineFreeLook>();
        freeLookCamera.Follow = player.transform.GetChild(2);
        freeLookCamera.LookAt = player.transform.GetChild(2);
    }

    public void RigisterObserver(IEndGameObserver gameObserver)
    {
        endGameObservers.Add(gameObserver);
    }

    public void RemoveObserver(IEndGameObserver gameObserver)
    {
        endGameObservers.Remove(gameObserver);
    }

    public bool ContainObserver(IEndGameObserver gameObserver)
    {
        return endGameObservers.Contains(gameObserver);
    }

    public void NotifyObserver()
    {
        foreach (IEndGameObserver gameObserver in endGameObservers)
        {
            gameObserver.EndNotify();
        }
    }
}

//test by xialjx
