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

        if (attackTarget == null) return;// 如果此时死亡，攻击无效
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // 如果此时玩家未处于前方，攻击无效
        if (!FoundPlayerInAttackRange()) return; // 如果此时玩家离开，攻击无效

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    public void SkillHit()
    {
        StartCoroutine(SlashPS2());

        if (attackTarget == null) return;// 如果此时死亡，攻击无效
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // 如果此时玩家未处于前方，攻击无效
        if (!FoundPlayerInAttackRange()) return; // 如果此时玩家离开，攻击无效

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
