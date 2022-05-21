using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GruntController : EnemyController
{
    public float kickForce = 15.0f;

    public void KickOff() // �����¼����������
    {
        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // �����ʱ���δ����ǰ����������Ч
        if (!FoundPlayerInSkillRange()) return; // �����ʱ����뿪��������Ч
        
        transform.LookAt(attackTarget.transform);

        Vector3 direction = attackTarget.transform.position - transform.position;
        direction.Normalize();

        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<NavMeshAgent>().velocity = direction * kickForce;
        attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");
       
    }
}
