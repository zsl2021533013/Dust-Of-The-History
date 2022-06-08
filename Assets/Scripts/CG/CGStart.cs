using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CGStart : MonoBehaviour
{
    public GameObject cgManager;

    public AudioClip scene_3_BGM;

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
            {
                BGMManager.Instance.PlayBGM(scene_3_BGM);
                rDemoCGLogic.isStart = true;
            }
                
            cgManager.GetComponent<CameraManager>().BroadcastCG(CGCameraID, forwardTime, cameraSpeed);
            Destroy(gameObject);
        }
    }
}
