using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RDemoCGLogic2 : MonoBehaviour
{
    public GameObject player;

    public GameObject redDemo2;

    public GameObject weapon;

    public GameObject power;

    public GameObject nuke;

    public GameObject weaponCamera;

    public Transform endPos;

    float speed = 10.0f;

    float passTime = 0.0f;

    bool isStart = true;

    NavMeshAgent navMeshAgent;

    CameraManager cameraManager;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        navMeshAgent = player.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isRedDemoDead && isStart)
        {
            passTime += Time.deltaTime;
            if (passTime > 4.0f)
            {
                if (4.0f < passTime && passTime < 4.1f)
                    cameraManager.BroadcastCG(0, 6.0f, 1.0f);
                if (passTime > 7.0f)
                    power.SetActive(true);
                if (passTime > 14.0f)
                {
                    if (weapon.transform.position != endPos.position)
                    {
                        navMeshAgent.isStopped = true;
                        speed += 30.0f * Time.deltaTime;
                        weapon.transform.position = Vector3.MoveTowards(weapon.transform.position, endPos.position, speed * Time.deltaTime);
                    }
                    else
                    {
                        weapon.SetActive(false);
                        nuke.SetActive(true);
                    }
                }
                if (14.0f < passTime && passTime < 14.1f)
                    cameraManager.MoveCG(0, 4.2f, 100.0f);
                if (passTime > 25.0f)
                    redDemo2.SetActive(true);
                if (passTime>27.0f)
                {
                    cameraManager.EndCG(4.0f, 1.0f);
                    navMeshAgent.isStopped = false;
                    isStart = false;
                }
            }
        }
    }
}
