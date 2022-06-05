using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform mainCamera;

    public GameObject cgCamera;

    public Transform[] cameraPos;

    public float cgCameraSpeed = 1.0f;

    public float cgCameraShakeRange = 0.1f;

    bool isForward = false;

    bool isBack = false;

    float passTime = 0.0f;

    int index;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (isForward)
        {
            passTime += Time.deltaTime;
            cgCamera.transform.position = Vector3.Lerp(cgCamera.transform.position, cameraPos[index].position, cgCameraSpeed * Time.deltaTime);
            cgCamera.transform.rotation = Quaternion.Slerp(cgCamera.transform.rotation, cameraPos[index].rotation, 1.0f * Time.deltaTime);
            if (passTime > 6.0f)
                isForward = false;
        }
        if(isBack)
        {
            passTime += Time.deltaTime;
            cgCamera.transform.position = Vector3.Lerp(cgCamera.transform.position, mainCamera.position, 2.0f * passTime * cgCameraSpeed * Time.deltaTime);
            cgCamera.transform.rotation = Quaternion.Lerp(cgCamera.transform.rotation, mainCamera.rotation, 2.0f * passTime * cgCameraSpeed * Time.deltaTime);
            if (Vector3.Distance(cgCamera.transform.position, mainCamera.position)<0.3f) 
            {
                isBack = false;
                cgCamera.SetActive(false);
            }
        }
    }

    public void BroadcastCG(int id)
    {
        passTime = 0.0f;
        index = id;
        cgCamera.transform.position = mainCamera.transform.position;
        cgCamera.transform.rotation = mainCamera.transform.rotation;
        cgCamera.SetActive(true);
        isForward = true;
    }

    public void EndCG()
    {
        passTime = 0.0f;
        isBack = true;
    }
}
