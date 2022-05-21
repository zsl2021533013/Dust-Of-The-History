using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour
{
    public float time = 3.0f;//�����A�������B������ʱ��

    public int damage = 10;

    [HideInInspector]
    public Transform pointA;//��A

    [HideInInspector]
    public Transform pointB;//��B

    private float g = -10.0f;//�������ٶ�

    private Vector3 speed;//���ٶ�����

    private Vector3 Gravity;//��������

    private Vector3 currentAngle;

    private Vector3 dir;

    void Start()
    {
        pointA = transform; //�� A ����
        pointB = GameManager.Instance.player.transform; //���� B ��

        //������ٶ�
        speed = new Vector3((pointB.position.x - pointA.position.x) / time,
            (pointB.position.y - pointA.position.y) / time - 0.5f * g * time, (pointB.position.z - pointA.position.z) / time);
        Gravity = Vector3.zero;
        currentAngle = Vector3.zero;

        Destroy(gameObject, 10.0f);
    }

    void FixedUpdate()
    {
        Gravity.y += g * Time.fixedDeltaTime;
        transform.position += (speed + Gravity) * Time.fixedDeltaTime;

        transform.forward = (speed + Gravity).normalized;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.characterStats.TakeDamage(damage, GameManager.Instance.characterStats);
            GameManager.Instance.player.GetComponent<Animator>().SetTrigger("Hit");
            Destroy(gameObject);
        }
    }
}