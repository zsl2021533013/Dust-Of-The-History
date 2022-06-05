using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RDemoCGLogic : MonoBehaviour
{
    public GameObject player;

    public GameObject redDemo;

    public GameObject fire;

    public GameObject[] magic;

    public GameObject[] power;

    public GameObject[] energy;

    public GameObject[] laser;

    public GameObject portal;

    public Light groundLight;

    public Light light1;

    public Light light2;

    NavMeshAgent navMeshAgent;

    CameraManager cameraManager;

    [HideInInspector]
    public bool isStart = false;

    float timeLine = -5.0f;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        navMeshAgent = player.GetComponent<NavMeshAgent>();
        foreach (GameObject i in magic)
        {
            float a = Random.Range(-10.0f, 10.0f), b = Random.Range(-10.0f, 10.0f), c = Random.Range(-10.0f, 10.0f);
            i.transform.rotation = Quaternion.Euler(0.0f + a, 0.0f + b, -180.0f + c);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            timeLine += Time.deltaTime;
            if (1.0f < timeLine && timeLine < 3.0f)
            {
                light1.intensity += 5000 * Time.deltaTime;
                light2.intensity += 5000 * Time.deltaTime;
            }
            if (3.0f < timeLine && timeLine < 6.5f) 
            {
                light1.intensity += 30000 * Time.deltaTime;
                light2.intensity += 30000 * Time.deltaTime;
            }
            if (2.5f < timeLine && timeLine < 8.0f) 
            {
                fire.SetActive(true);
                fire.transform.localScale += new Vector3(9.0f * Time.deltaTime, 9.0f * Time.deltaTime, 9.0f * Time.deltaTime);
            }
            if (6.0f < timeLine && timeLine < 6.1f) 
                cameraManager.MoveCG(1, 4.0f, 0.8f);
            if (timeLine > 10.0f)
            {
                for (int i = 0; i < magic.Length; i++)
                    if (timeLine > 10.0f + i * 0.8f)
                        magic[i].SetActive(true);
            }
            if (10.0f < timeLine && timeLine < 13.0f) 
            {
                for (int i = 0; i <= 1; i++)
                {
                    power[i].SetActive(true);
                    energy[i].SetActive(true);
                    energy[i].transform.localScale += new Vector3(6.0f * Time.deltaTime, 6.0f * Time.deltaTime, 6.0f * Time.deltaTime);
                }
            }
            if(timeLine>15.0f)
            {
                foreach (GameObject i in laser)
                    i.SetActive(true);
                groundLight.intensity = 9999;
            }
            if (20.0f < timeLine && timeLine < 24.0f) 
            {
                portal.SetActive(true);
                portal.transform.localScale += new Vector3(2.0f * Time.deltaTime, 2.0f * Time.deltaTime, 2.0f * Time.deltaTime);
            }
            if (19.0f < timeLine && timeLine < 19.1f)
                cameraManager.MoveCG(2, 5, 0.7f);
            if(timeLine>26.0f)
            {
                redDemo.SetActive(true);
                portal.transform.localScale -= new Vector3(2.0f * Time.deltaTime, 2.0f * Time.deltaTime, 2.0f * Time.deltaTime);
                groundLight.intensity -= 99999 * Time.deltaTime;
            }
            if (timeLine>28.0f)
            {
                light1.enabled = false;
                light2.enabled = false;
                fire.SetActive(false);
                foreach (GameObject i in magic)
                    i.SetActive(false);
                foreach (GameObject i in power)
                    i.SetActive(false);
                foreach (GameObject i in energy)
                    i.SetActive(false);
                foreach (GameObject i in laser)
                    i.SetActive(false);
            }
            if(timeLine>30.0f)
            {
                portal.SetActive(false);
                groundLight.enabled = false;
                isStart = false;
                cameraManager.EndCG(3.0f, 1.0f);
                navMeshAgent.isStopped = false;
            }
        }
    }
}
