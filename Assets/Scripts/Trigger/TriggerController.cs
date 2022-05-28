using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriggerController : MonoBehaviour
{
    InteriorController[] interiorController;

    [Space(10)]
    [Header("1.��������")]
    public GameObject handle;

    float triggerTime = 0.0f;

    public bool isActive = false; //trigger�Ƿ񴥷�

    //Trigger�Ĳ�ͬ����

    [Space(10)]
    [Header("2.������")]
    public bool doorAction = true;

    public GameObject[] door;

    public float[] rotateEndPos;

    public float doorCloseSpeed = 0.5f; //���ŵ�����ٶ�

    [Space(10)]
    [Header("3.���������ſɴ���")]
    public bool enemyLink = false;

    [SerializeField]
    public bool isEnemyDead;

    void Start()
    {
        /*if (doorAction)
        {
            interiorController = new InteriorController[door.Length];
            for (int i = 0; i < door.Length; i++) 
                interiorController[i] = door[i].GetComponent<InteriorController>();
            Debug.Log(interiorController[0]);
            Debug.Log(interiorController[1]);
        }*/
    }

    void Update()
    {
        if (enemyLink)
            if (!isEnemyDead)
                return;
        if (isActive)
        {
            SetTrigger();
            if (doorAction) RotateDoor();
            triggerTime += Time.deltaTime;
            if (triggerTime > 10.0f) isActive = false;
        }
    }

    void SetTrigger()
    {
        handle.transform.rotation = Quaternion.Slerp(handle.transform.rotation, Quaternion.Euler(50.0f, 0.0f, 0.0f), 0.5f * Time.deltaTime);
    }
    void RotateDoor()
    {
        for (int i = 0; i < door.Length; i++)
        {
            door[i].transform.rotation = Quaternion.Slerp(door[i].transform.rotation, Quaternion.Euler(0.0f, rotateEndPos[i], 0.0f), doorCloseSpeed * Time.deltaTime);
            //interiorController[i].BulidNavMesh();
        }
    }

    void OnTriggerStay(Collider other)
    {
        //Debug.Log(MouseManager.Instance.isClickTrigger);
        if (other.CompareTag("Player") && MouseManager.Instance.isClickTrigger)
        {
            isActive = true;
            triggerTime = 0.0f;
        }
    }


}
