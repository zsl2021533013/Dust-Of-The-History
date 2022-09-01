using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CGStart : MonoBehaviour
{
    public GameObject cgManager;

    public int CGCameraID;

    public float forwardTime;

    public float cameraSpeed;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<NavMeshAgent>().isStopped = true;

            if (cgManager.TryGetComponent<DemoCGLogic>(out DemoCGLogic demoCGLogic)) 
                demoCGLogic.isStart = true;

            if(cgManager.TryGetComponent<RDemoCGLogic>(out RDemoCGLogic rDemoCGLogic))
                rDemoCGLogic.isStart = true;

            cgManager.GetComponent<CameraManager>().BroadcastCG(CGCameraID, forwardTime, cameraSpeed);
            Destroy(gameObject);
        }
    }
}
