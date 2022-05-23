using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MachanicalGolem : EnemyController
{
    public GameObject slashPS1;

    public GameObject slashPS2;

    public override void Hit()
    {
        StartCoroutine(SlashPS1());

        if (attackTarget == null) return;// �����ʱ������������Ч
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // �����ʱ���δ����ǰ����������Ч
        if (!FoundPlayerInAttackRange()) return; // �����ʱ����뿪��������Ч

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    public void SkillHit()
    {
        StartCoroutine(SlashPS2());

        if (attackTarget == null) return;// �����ʱ������������Ч
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // �����ʱ���δ����ǰ����������Ч
        if (!FoundPlayerInAttackRange()) return; // �����ʱ����뿪��������Ч

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    IEnumerator SlashPS1()
    {
        slashPS1.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        slashPS1.SetActive(false);
        yield break;
    }

    IEnumerator SlashPS2()
    {
        slashPS2.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        slashPS2.SetActive(false);
        yield break;
    }
}
