using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MachanicalGolem : EnemyController
{
    [Header("Attack 1 & 2 Information")]

    public int minFlameDamage;

    public int maxFlameDamage;

    public int minShootDamage;
    
    public int maxShootDamage;

    public float dotThreshold;

    public float rotSpeed;

    public GameObject flameThrowerPS;

    public GameObject gunShotPS;

    [Header("Skill Information")]

    public int skill2Damage;

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
                return true;
            }
        }

        return false;
    }

    public override void Hit()
    {
        StartCoroutine(FlameThrowerPS());
    }

    public void SkillHit()
    {
        StartCoroutine(GunShotPS());
    }

    public void Skill2Hit()
    {
        StartCoroutine(PowerDrawPS());
    }

    IEnumerator FlameThrowerPS()
    { 
        StartCoroutine("BurnPlayer");
        StartCoroutine("FacingTowardsPlayerPS");
        flameThrowerPS.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        StopCoroutine("BurnPlayer");
        StopCoroutine("FacingTowardsPlayerPS");
        flameThrowerPS.SetActive(false);
        yield break;
    }

    IEnumerator GunShotPS()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("ShootPlayer");
        StartCoroutine("FacingTowardsPlayerPS");
        gunShotPS.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        StopCoroutine("ShootPlayer");
        StopCoroutine("FacingTowardsPlayerPS");
        gunShotPS.SetActive(false);
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

    IEnumerator BurnPlayer()
    {
        while (true)
        {
            if (!GameManager.Instance.gameObject)
            {
                yield break;
            }

            Vector3 dir = GameManager.Instance.player.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();

            if (Vector3.Dot(dir, transform.forward) > dotThreshold && FoundPlayerInAttackRange()) // 如果此时在扇形范围内
            {
                GameManager.Instance.characterStats.TakeDamage((int)Random.Range(minFlameDamage, maxFlameDamage), GameManager.Instance.characterStats);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator ShootPlayer()
    {
        while (true)
        {
            if (!GameManager.Instance.gameObject)
            {
                yield break;
            }

            Vector3 dir = GameManager.Instance.player.transform.position - transform.position;
            dir.y = 0;
            dir.Normalize();

            if (Vector3.Dot(dir, transform.forward) > dotThreshold && FoundPlayerInSkillRange()) // 如果此时在扇形范围内
            {
                GameManager.Instance.characterStats.TakeDamage((int)Random.Range(minShootDamage, maxShootDamage), GameManager.Instance.characterStats);
            }

            yield return new WaitForSeconds(0.1f);
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
}
