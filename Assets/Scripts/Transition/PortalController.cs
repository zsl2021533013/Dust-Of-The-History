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

    [Header("Link to enemy death")]

    public bool enemyLink = false;

    public EnemyType enemyType;

    public float endPos = 13.0f;

    public float portalSpeed = 10.0f;

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
                gameObject.transform.position = new Vector3(
                        gameObject.transform.position.x,
                        Mathf.Lerp(gameObject.transform.position.y, endPos, portalSpeed * Time.deltaTime),
                        gameObject.transform.position.z);
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
