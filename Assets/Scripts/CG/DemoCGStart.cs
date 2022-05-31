using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoCGStart : MonoBehaviour
{
    public GameObject cgManager;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<NavMeshAgent>().isStopped = true;
            cgManager.GetComponent<DemoCGLogic>().isStart = true;
            cgManager.GetComponent<CameraManager>().BroadcastCG(0);
            Destroy(gameObject);
        }
    }
}
