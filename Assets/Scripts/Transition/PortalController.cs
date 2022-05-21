using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType
{
    SameScene,
    DifferentScene
}

public class PortalController : MonoBehaviour
{
    [Header("Transition Information")]
    public string sceneName;
    public TransitionType transitionType;
    public DestinationTag destinationTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) // 不能用协程，因为会开很多个出来！
    {
        if (other.CompareTag("Player") && MouseManager.Instance.clickPortal)
        {
            TranstionManager.Instance.TransitionToDestination(this);
        }
    }
}
