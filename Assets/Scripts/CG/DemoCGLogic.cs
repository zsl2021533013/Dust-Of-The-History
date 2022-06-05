using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemoCGLogic : MonoBehaviour
{
    public GameObject player;

    public GameObject demo;

    public GameObject fire1;

    public GameObject fire2;

    public GameObject lightBall;

    public GameObject[] lightning;

    public GameObject[] groundFire;

    NavMeshAgent navMeshAgent;

    CameraManager cameraManager;

    [HideInInspector]
    public bool isStart = false;

    float timeLine = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        navMeshAgent = player.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isStart)
        {
            timeLine += Time.deltaTime;
            if (timeLine > 2.0f) fire1.SetActive(true);
            if (timeLine > 3.0f) fire2.SetActive(true);
            if (timeLine > 4.0f) lightBall.SetActive(true);
            if (timeLine > 9.0f) 
                for (int i = 0; i < lightning.Length; i++)
                    if (timeLine > 9.0f + i * 0.1f) lightning[i].SetActive(true);
            if (timeLine > 9.5f)
                for (int i = 0; i < groundFire.Length; i++)
                    groundFire[i].SetActive(true);
            if (timeLine > 10.0f) demo.SetActive(true);
            if (timeLine > 11.0f)
            {
                isStart = false;
                cameraManager.EndCG();
                navMeshAgent.isStopped = false;
            }
        }
    }

}
