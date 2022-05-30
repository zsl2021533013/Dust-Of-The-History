using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedDemonController : EnemyController
{
    public float Attack2CoolDown;

    public float attack2Range;

    private float attack2CoolDown;

    public GameObject groundCrackPS;

    public GameObject rockPS;

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
        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return; 
        if (!FoundPlayerInAttackRange()) return; 

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    public void SkillAttack()
    {
        StartCoroutine(RockPS());

        if (attackTarget == null) return;
        if (!FoundPlayerInAttack2Range()) return;

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("Knockdown");
    }

    public void Hit2()
    {
        StartCoroutine(GroundCrackPS());

        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return;
        if (!FoundPlayerInAttack2Range()) return;

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("Hit");
    }

    IEnumerator GroundCrackPS()
    {
        groundCrackPS.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        groundCrackPS.SetActive(false);
        yield break;
    }

    IEnumerator RockPS()
    {
        rockPS.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        rockPS.SetActive(false);
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
