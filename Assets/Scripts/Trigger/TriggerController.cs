using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TriggerController : MonoBehaviour
{
    public enum EnemyType
    {
        Knight,
        Demo,
        MachineGolem,
        RedDemo
    }

    [Space(10)]
    [Header("1.基本设置")]
    public GameObject handle;

    float passTime = 0.0f;

    [HideInInspector]
    public bool isActive = false; //trigger是否触发

    //Trigger的不同功能

    [Space(10)]
    [Header("2.开关门")]

    public bool doorAction = true;

    public GameObject[] door;

    public float[] rotateEndPos;

    public GameObject doorBlock;

    public float openTime = 1.0f;

    public float doorCloseSpeed = 0.5f; //关门的相对速度

    [Space(10)]
    [Header("3.敌人死亡才可触发")]
    public bool enemyLink = false;

    public EnemyType enemyType;

    [Space(10)]
    [Header("4.传送门")]
    public bool portalLink = false;

    public GameObject portal;

    PortalController portalController;

    void Start()
    {
        portalController = portal.GetComponent<PortalController>();
    }

    void Update()
    {
        if (enemyLink)
        {
            if (enemyType == EnemyType.Knight && !GameManager.Instance.isKnightDead)
                return;
        }
        if (isActive)
        {
            passTime += Time.deltaTime;
            SetTrigger();
            if (doorAction)
            {
                RotateDoor();
                if (passTime > openTime && doorBlock)
                    Destroy(doorBlock);
            }
            if (portalLink)
                portalController.SetTrigger();
            if (passTime > 6.0f)
                isActive = false;
        }
    }

    void SetTrigger()
    {
        handle.transform.rotation = Quaternion.Slerp(handle.transform.rotation, Quaternion.Euler(50.0f, 0.0f, 0.0f), 0.5f * Time.deltaTime);
    }
    void RotateDoor()
    {
        for (int i = 0; i < door.Length; i++)
            door[i].transform.rotation = Quaternion.Slerp(door[i].transform.rotation, Quaternion.Euler(0.0f, rotateEndPos[i], 0.0f), doorCloseSpeed * Time.deltaTime);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && MouseManager.Instance.isClickTrigger)
        {
            isActive = true;
            passTime = 0.0f;
        }
    }


}
