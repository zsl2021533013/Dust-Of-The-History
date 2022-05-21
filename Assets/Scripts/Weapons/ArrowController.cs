using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour
{
    public float time = 3.0f;//代表从A点出发到B经过的时长

    public int damage = 10;

    [HideInInspector]
    public Transform pointA;//点A

    [HideInInspector]
    public Transform pointB;//点B

    private float g = -10.0f;//重力加速度

    private Vector3 speed;//初速度向量

    private Vector3 Gravity;//重力向量

    private Vector3 currentAngle;

    private Vector3 dir;

    void Start()
    {
        pointA = transform; //从 A 出发
        pointB = GameManager.Instance.player.transform; //飞至 B 点

        //计算初速度
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