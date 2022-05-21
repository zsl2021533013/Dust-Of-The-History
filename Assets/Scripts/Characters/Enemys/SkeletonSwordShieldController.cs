using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonSwordShieldController : EnemyController
{
    public float kickForce = 15.0f;

    public GameObject slashPS; 

    public override void Hit()
    {
        StartCoroutine(SlashPS());
        base.Hit();
    }

    public void ShieldHit() // �ܻ�
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

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    IEnumerator SlashPS()
    {
        slashPS.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        slashPS.SetActive(false);
        yield break;
    }
}
