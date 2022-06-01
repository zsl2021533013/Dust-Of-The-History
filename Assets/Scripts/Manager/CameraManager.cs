using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject mainCamera;

    public GameObject cgCamera;

    public GameObject[] cameraPos;

    public float cgCameraSpeed = 1.0f;

    public float cgCameraShakeRange = 0.1f;

    bool isForward = false;

    bool isBack = false;

    bool isShake = false;

    float passTime = 0.0f;

    float maxShakeTime = 0.0f;

    float initialX;

    int shakeDir = 1;

    int index;

    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (isForward)
        {
            cgCamera.transform.position = new Vector3(
                Mathf.Lerp(cgCamera.transform.position.x, cameraPos[index].transform.position.x, cgCameraSpeed * Time.deltaTime),
                Mathf.Lerp(cgCamera.transform.position.y, cameraPos[index].transform.position.y, cgCameraSpeed * Time.deltaTime),
                Mathf.Lerp(cgCamera.transform.position.z, cameraPos[index].transform.position.z, cgCameraSpeed * Time.deltaTime));
            cgCamera.transform.rotation = Quaternion.Slerp(cgCamera.transform.rotation, cameraPos[index].transform.rotation, 0.5f * Time.deltaTime);
            if (passTime > 3.0f)
                isForward = false;
        }
        if(isBack)
        {
            passTime += Time.deltaTime;
            cgCamera.transform.position = new Vector3(
                Mathf.Lerp(cgCamera.transform.position.x, mainCamera.transform.position.x, cgCameraSpeed * Time.deltaTime),
                Mathf.Lerp(cgCamera.transform.position.y, mainCamera.transform.position.y, cgCameraSpeed * Time.deltaTime),
                Mathf.Lerp(cgCamera.transform.position.z, mainCamera.transform.position.z, cgCameraSpeed * Time.deltaTime));
            cgCamera.transform.rotation = Quaternion.Slerp(cgCamera.transform.rotation, mainCamera.transform.rotation, 1.0f * Time.deltaTime);
            if (passTime > 10.0f)
            {
                isBack = false;
                cgCamera.SetActive(false);
            }    
        }
        /*if(isShake)
        {
            passTime += Time.deltaTime;

            if ((shakeDir == 1 && cgCamera.transform.position.x - initialX < cgCameraShakeRange)
                || (shakeDir == -1 && initialX - cgCamera.transform.position.x  < cgCameraShakeRange)) 
                    cgCamera.transform.Translate(shakeDir * 1.0f * Time.deltaTime, 0.0f, 0.0f);
            else
                shakeDir *= -1;
            if (passTime > maxShakeTime)
                isShake = false;
        }*/
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

    public void ShakeCamera(float maxTime)
    {
        initialX = cgCamera.transform.position.x;
        //passTime = 0.0f;
        maxShakeTime = maxTime;
        isShake = true;
    }
}
