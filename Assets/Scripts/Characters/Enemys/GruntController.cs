using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntController : EnemyController
{
    public float kickForce = 15.0f;

    public void KickOff() // 动画事件，击飞玩家
    {
        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // 如果此时玩家未处于前方，攻击无效
        if (!FoundPlayerInSkillRange()) return; // 如果此时玩家离开，攻击无效
        
        transform.LookAt(attackTarget.transform);

        Vector3 direction = attackTarget.transform.position - transform.position;
        direction.Normalize();

        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
        attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
       
    }
}
