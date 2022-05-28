using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MachanicalGolem : EnemyController
{
    [Header("Attack 1 & 2 Information")]

    public float rotSpeed;

    public GameObject flameThrowerPS;

    public GameObject groundCrackPS;

    [Header("Skill Information")]

    public float skill2Range;

    public float Skill2CoolDown = 18.0f;

    public GameObject powerDrawPS;

    private float skill2CoolDown = 0.0f;

    public bool FoundPlayerInSkill2Range()
    {
        var colliders = Physics.OverlapSphere(transform.position, skill2Range);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                attackTarget = collider.gameObject;
                return !FoundPlayerInAttackRange();
            }
        }

        return false;
    }

    public void SkillHit()
    {
        StartCoroutine(FlameThrowerPS());
    }

    public override void Hit() 
    {
        StartCoroutine(GroundCrackPS());

        if (attackTarget == null) return;
        if (!FoundPlayerInAttackRange()) return;

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("Knockdown");
    }

    public void Skill2Hit()
    {
        StartCoroutine(PowerDrawPS());
    }

    IEnumerator FlameThrowerPS()
    { 
        StartCoroutine("FacingTowardsPlayerPS");
        flameThrowerPS.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        StopCoroutine("FacingTowardsPlayerPS");
        flameThrowerPS.SetActive(false);
        yield break;
    }

    IEnumerator GroundCrackPS()
    {
        groundCrackPS.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        groundCrackPS.SetActive(false);
        yield break;
    }

    IEnumerator PowerDrawPS()
    {
        powerDrawPS.SetActive(true);
        animator.SetBool("IsPowerDraw", true);
        StartCoroutine("FacingTowardsPlayerPS");
        yield return new WaitForSeconds(7.0f);
        animator.SetBool("IsPowerDraw",false);
        powerDrawPS.SetActive(false);
        StopCoroutine("FacingTowardsPlayerPS");
        yield break;
    }

    IEnumerator FacingTowardsPlayerPS() // TODO: 效果不好啊
    {
        while (true)
        {
            Transform targetPos = GameManager.Instance.player.transform;
            yield return new WaitForSeconds(0.3f);
            Quaternion targetRot = Quaternion.LookRotation(targetPos.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
        }
    }

    protected override void ExitChaseState()
    {
        if (FoundPlayerInAttackRange() || FoundPlayerInSkillRange() || (FoundPlayerInSkill2Range() && skill2CoolDown <= 0))
        {
            agent.destination = transform.position;
            enemyState = EnemyState.ATTCK;
        }

        if (!FoundPlayerInSightRange())
        {
            agent.destination = transform.position;
            enemyState = originalState;
        }
    }

    protected override void EnterAttackState()
    {
        base.EnterAttackState();

        if (skill2CoolDown > 0)
        {
            skill2CoolDown -= Time.deltaTime;
        }
        else if (FoundPlayerInSkill2Range())
        {
            transform.LookAt(attackTarget.transform);
            characterStats.isCritical = (Random.value < characterStats.CriticalChance);
            animator.SetBool("Critical", characterStats.isCritical);
            animator.SetTrigger("Skill2Attack");
            skill2CoolDown = Skill2CoolDown;
        }
    }

    public override void ExitAttackState()
    {
        if (!(FoundPlayerInAttackRange() || FoundPlayerInSkillRange() || (FoundPlayerInSkill2Range() && skill2CoolDown <= 0)))
        {
            if (FoundPlayerInSightRange())
            {
                enemyState = EnemyState.CHASE;
            }
            else
            {
                enemyState = originalState;
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(transform.position, 6.0f);
    //}
}
