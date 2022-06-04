using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType
{
    SameScene,
    DifferentScene
}
public enum EnemyType
{
    Knight,
    Demo,
    MachineGolem,
    RedDemo
}

public class PortalController : MonoBehaviour
{
    [Header("Transition Information")]
    public string sceneName;

    public TransitionType transitionType;

    public DestinationTag destinationTag;

    [Header("Movement")]

    public GameObject portal;

    public GameObject dust;

    public Transform endPos;

    public float portalSpeed = 1.0f;

    [Header("Link to enemy death")]
    public bool enemyLink = false;

    public EnemyType enemyType;

    float passTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyLink)
        {
            if (enemyType == EnemyType.Demo && GameManager.Instance.isDemoDead)
            {
                dust.SetActive(true);
                passTime += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, endPos.position, portalSpeed * Time.deltaTime);
                if (passTime > 5.0f)
                    portal.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other) // 不能用协程，因为会开很多个出来！
    {
        if (other.CompareTag("Player") && MouseManager.Instance.isClickPortal)
        {
            TranstionManager.Instance.TransitionToDestination(this);
        }
    }
}
