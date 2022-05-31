using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCamera;

    public GameObject cgCamera;

    public GameObject[] cameraPos;

    public float cgCameraSpeed = 1.0f;

    bool isForward = false;

    bool isBack = false;

    int index;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
        if(isForward)
        {
            cgCamera.transform.position = new Vector3(
                Mathf.Lerp(cgCamera.transform.position.x, cameraPos[index].transform.position.x, cgCameraSpeed * Time.deltaTime), 
                Mathf.Lerp(cgCamera.transform.position.y, cameraPos[index].transform.position.y, cgCameraSpeed * Time.deltaTime),
                Mathf.Lerp(cgCamera.transform.position.z, cameraPos[index].transform.position.z, cgCameraSpeed * Time.deltaTime));
            cgCamera.transform.rotation = Quaternion.Slerp(cgCamera.transform.rotation, cameraPos[index].transform.rotation, 0.5f * Time.deltaTime);
        }
        if(isBack)
        {
            cgCamera.transform.position = new Vector3(
                Mathf.Lerp(cgCamera.transform.position.x, mainCamera.transform.position.x, cgCameraSpeed * Time.deltaTime),
                Mathf.Lerp(cgCamera.transform.position.y, mainCamera.transform.position.y, cgCameraSpeed * Time.deltaTime),
                Mathf.Lerp(cgCamera.transform.position.z, mainCamera.transform.position.z, cgCameraSpeed * Time.deltaTime));
            cgCamera.transform.rotation = Quaternion.Slerp(cgCamera.transform.rotation, mainCamera.transform.rotation, 0.5f * Time.deltaTime);
            if(cgCamera.transform.position== mainCamera.transform.position)
            {
                isBack = false;
                cgCamera.SetActive(false);
            }    
        }
    }

    public void BroadcastCG(int id)
    {
        index = id;
        cgCamera.transform.position = mainCamera.transform.position;
        cgCamera.transform.rotation = mainCamera.transform.rotation;
        cgCamera.SetActive(true);
        isForward = true;
    }

    public void EndCG()
    {
        isForward = false;
        isBack = true;
    }
}
