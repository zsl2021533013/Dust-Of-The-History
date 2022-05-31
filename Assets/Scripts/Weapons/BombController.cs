using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float time = 3.0f;//�����A�������B������ʱ��

    public int damage = 10;

    public float dis;

    public float g = -10.0f;//�������ٶ�
    
    public GameObject explosionPS;

    [HideInInspector]
    public Vector3 pointA;//��A

    [HideInInspector]
    public Vector3 pointB;//��B

    private Vector3 speed;//���ٶ�����

    private Vector3 Gravity;//��������

    private Vector3 currentAngle;

    private Vector3 dir;

    private bool isMove = true;

    void Start()
    {
        dir = transform.forward;
        dir.y = 0;
        dir.Normalize();

        pointA = transform.position; //�� A ����
        pointB = transform.position + dis * dir; //���� B ��

        //������ٶ�
        speed = new Vector3((pointB.x - pointA.x) / time,
            (pointB.y - pointA.y) / time - 0.5f * g * time, (pointB.z - pointA.z) / time);
        Gravity = Vector3.zero;
        currentAngle = Vector3.zero;

        Destroy(gameObject, 10.0f);
    }

    void FixedUpdate()
    {
        if (isMove)
        {
            Gravity.y += g * Time.fixedDeltaTime;
            transform.position += (speed + Gravity) * Time.fixedDeltaTime;

            transform.up = (speed + Gravity).normalized;
        }
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        isMove = false;
        StartCoroutine(Explose());
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<CharacterStats>().TakeDamage(damage, collision.gameObject.GetComponent<CharacterStats>());
            collision.gameObject.GetComponent<Animator>().SetTrigger("Knockdown");
        }
    }

    IEnumerator Explose()
    {
        explosionPS.SetActive(true);
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
        yield break;
    }
}
