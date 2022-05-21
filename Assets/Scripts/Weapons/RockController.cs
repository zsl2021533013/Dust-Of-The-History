using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum RockState
{
    HitPlayer,
    HitNothing,
    HitEnemy
}

public class RockController : MonoBehaviour
{
    Rigidbody rb;

    public GameObject target;

    public float throwForce = 10.0f; // 扔出力量，影响扔出速度

    public float kickForce = 10.0f; // 打击力量，影响玩家被击退的距离

    public int damageForPlayer = 10;

    public int damageForGolem = 15;

    public RockState rockState;

    public GameObject rockBreak;

    //Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        FlyToTarget();
    }

    void FlyToTarget()
    {
        Vector3 dir = target.transform.position - transform.position + 2 * Vector3.up;
        dir.Normalize();
        rb.AddForce(dir * throwForce, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        switch (rockState)
        {
            case RockState.HitPlayer:
                if (other.gameObject.CompareTag("Player"))
                {
                    Vector3 dir = other.transform.position - transform.position;
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    other.gameObject.GetComponent<NavMeshAgent>().velocity = dir * kickForce;

                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damageForPlayer, other.gameObject.GetComponent<CharacterStats>());

                    other.gameObject.GetComponent<Animator>().SetTrigger("Dizzy");
                }
                if (!other.gameObject.CompareTag("Enemy"))
                {
                    rockState = RockState.HitNothing;
                }
                break;

            case RockState.HitNothing:

                break;

            case RockState.HitEnemy:
                if (other.gameObject.GetComponent<GolemController>())
                {
                    other.gameObject.GetComponent<CharacterStats>().TakeDamage(damageForGolem, other.gameObject.GetComponent<CharacterStats>());
                    Instantiate(rockBreak, transform.position, Quaternion.identity);
                    other.gameObject.GetComponent<Animator>().SetTrigger("Hit");
                    Destroy(gameObject);
                }
                else
                {
                    rockState = RockState.HitNothing;
                }
                break;
        }


    }

}
