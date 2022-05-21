using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GolemController : EnemyController
{
    public float kickForce = 25.0f;

    public Transform handPos;

    public GameObject rockPrefab;

    public void KickOff() // 动画事件，击飞玩家
    {
        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // 如果此时玩家未处于前方，攻击无效
        if (!FoundPlayerInAttackRange()) return; // 如果此时玩家离开，攻击无效
        

        transform.LookAt(attackTarget.transform);

        Hit();

        Vector3 direction = attackTarget.transform.position - transform.position;
        direction.Normalize();

        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce; // TODO: 石头人似乎无法击退玩家了
        attackTarget.GetComponent<NavMeshAgent>().enabled = true;
        attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

    }

    public void ThrowRock() // 动画事件，扔石头
    {
        if (attackTarget == null)
        {
            attackTarget = FindObjectOfType<PlayerController>().gameObject;
        }
        var rock = Instantiate(rockPrefab, handPos.position, Quaternion.identity);
        rock.GetComponent<RockController>().target = attackTarget;
    }
}
