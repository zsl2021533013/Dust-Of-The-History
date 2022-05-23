using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UndeadKnightController : EnemyController
{
    public const float KickCoolDown = 2.0f;

    public float kickRange = 1.3f;

    public float kickCoolDown = 3.0f;

    public float kickForce = 10.0f;

    public GameObject slashPS1;

    public GameObject slashPS2;

    public bool FoundPlayerInKickRange() // �߻�
    {
        var colliders = Physics.OverlapSphere(transform.position, kickRange);

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
        StartCoroutine(SlashPS1());

        if (attackTarget == null) return;// �����ʱ������������Ч
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // �����ʱ���δ����ǰ����������Ч
        if (!FoundPlayerInAttackRange()) return; // �����ʱ����뿪��������Ч

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("Hit");
    }

    public void SkillHit()
    {
        StartCoroutine(SlashPS2());

        if (attackTarget == null) return;// �����ʱ������������Ч
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // �����ʱ���δ����ǰ����������Ч
        if (!FoundPlayerInAttackRange()) return; // �����ʱ����뿪��������Ч

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        //characterStats.isCritical = true;
        characterStats.TakeDamage((int)(characterStats.MaxDamge * characterStats.CriticalMultiplier), targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("Knockdown");
    }

    public void Kick()
    {
        if (attackTarget == null) return;// �����ʱ������������Ч
        if (!transform.IsFacingTarget(attackTarget.transform)) return; // �����ʱ���δ����ǰ����������Ч
        if (!FoundPlayerInKickRange()) return; // �����ʱ����뿪��������Ч

        transform.LookAt(attackTarget.transform);

        Vector3 direction = attackTarget.transform.position - transform.position;
        direction.Normalize();

        attackTarget.GetComponent<NavMeshAgent>().isStopped = true;
        attackTarget.GetComponent<Rigidbody>().velocity = direction * kickForce;

        attackTarget.GetComponent<Animator>().SetTrigger("Dizzy");

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
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

        if(kickCoolDown > 0)
        {
            kickCoolDown -= Time.deltaTime;
        }
        else if(FoundPlayerInKickRange())
        {
            transform.LookAt(attackTarget.transform);
            characterStats.isCritical = (Random.value < characterStats.CriticalChance);
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Kick");
            kickCoolDown = KickCoolDown;
        }
    }
}
