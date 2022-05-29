using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDemonController : EnemyController
{
    public const float Attack2CoolDown = 2.0f;

    public float attack2Range = 1.3f;

    public float attack2CoolDown = 3.0f;

    public GameObject slashPS1;

    public GameObject slashPS2;

    public bool FoundPlayerInAttack2Range() 
    {
        var colliders = Physics.OverlapSphere(transform.position, attack2Range);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return true;
            }
        }

        return false;
    }

    public override void Hit()
    {
        //StartCoroutine(SlashPS1());

        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return; 
        if (!FoundPlayerInAttackRange()) return; 

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    public void SkillHit()
    {
        
    }

    public void Hit2()
    {
        //StartCoroutine(SlashPS2());

        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return;
        if (!FoundPlayerInAttack2Range()) return;

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("KnockDown");
    }

    IEnumerator SlashPS1()
    {
        slashPS1.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        slashPS1.SetActive(false);
        yield break;
    }

    IEnumerator SlashPS2()
    {
        slashPS2.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        slashPS2.SetActive(false);
        yield break;
    }

    protected override void EnterAttackState()
    {
        base.EnterAttackState();

        if (attack2CoolDown > 0)
        {
            attack2CoolDown -= Time.deltaTime;
        }
        else if (FoundPlayerInAttack2Range())
        {
            transform.LookAt(attackTarget.transform);
            characterStats.isCritical = (Random.value < characterStats.CriticalChance);
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Attack2");
            attack2CoolDown = Attack2CoolDown;
        }
    }
}
