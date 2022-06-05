using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform mainCamera;

    public GameObject cgCamera;

    public Transform[] cameraPos;

    float cgCameraSpeed = 1.0f;

    bool isForward = false;

    bool isBack = false;

    float passTime = 0.0f;

    float forwardTime, backTime;

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
            if (passTime > forwardTime)
                isForward = false;
        }
        if(isBack)
        {
            passTime += Time.deltaTime;
            cgCamera.transform.position = Vector3.Lerp(cgCamera.transform.position, mainCamera.position, cgCameraSpeed * Time.deltaTime);
            cgCamera.transform.rotation = Quaternion.Slerp(cgCamera.transform.rotation, mainCamera.rotation, 1.0f * Time.deltaTime);
            if (passTime > backTime)
            {
                isBack = false;
                cgCamera.SetActive(false);
            }    
        }
    }

    public void BroadcastCG(int id, float t, float speed)
    {
        passTime = 0.0f;
        index = id; forwardTime = t; cgCameraSpeed = speed;
        cgCamera.transform.position = mainCamera.transform.position;
        cgCamera.transform.rotation = mainCamera.transform.rotation;
        cgCamera.SetActive(true);
        isForward = true;
    }

    public void MoveCG(int id, float t, float speed)
    {
        passTime = 0.0f;
        index = id; forwardTime = t; cgCameraSpeed = speed;
        isForward = true;
    }

    public void EndCG(float t, float speed)
    {
        passTime = 0.0f;
        backTime = t; cgCameraSpeed = speed;
        isBack = true;
    }
}
