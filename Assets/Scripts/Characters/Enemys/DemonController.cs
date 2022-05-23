using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DemonController : EnemyController
{
    public const float Attack2CoolDown = 8.0f;

    public float attack2Range = 5.0f;

    public float attack2CoolDown = 0.0f;

    public GameObject slashPS1; // 攻击特效

    public GameObject slashPS2; // 攻击特效

    [Space(10)]

    [Header("Star Fall Information")]

    public GameObject lavaPS1Prefab; // 陨石术特效

    public GameObject lavaPS2Prefab; // 陨石术特效

    public GameObject star;

    public float offsetFactor = 4.0f;

    public float lavaExistTime = 10.0f;

    public int starAmount = 4;

    private Vector3 starPos; // 陨石术的目标点

    GameObject lavaPS1; // 陨石术特效

    GameObject lavaPS2;

    protected override void Update()
    {
        base.Update();
    }

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
        StartCoroutine(SlashPS1());

        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return; 
        if (!FoundPlayerInAttackRange()) return;

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);
    }

    public void Hit2() // 竖劈
    {
        StartCoroutine(SlashPS2());

        if (attackTarget == null) return;
        if (!transform.IsFacingTarget(attackTarget.transform)) return; 
        if (!FoundPlayerInAttackRange()) return; 

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("Hit");
    }

    public void StarFall() // 陨石坠落
    {
        StartCoroutine(CreateLava());

        for(int i = 0; i < starAmount; i++)
        {
            float randomX = GameManager.Instance.player.transform.position.x + offsetFactor * (Random.value - 0.5f); 
            float randomY = GameManager.Instance.player.transform.position.y + 10.0f + offsetFactor * Random.value;
            float randomZ = GameManager.Instance.player.transform.position.z + offsetFactor * (Random.value - 0.5f);
            starPos = new Vector3(randomX, randomY, randomZ);
            Instantiate(star, starPos, Quaternion.identity);
        }
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

    IEnumerator CreateLava()
    {
        lavaPS1 = Instantiate(lavaPS1Prefab, GameManager.Instance.player.transform.position, Quaternion.identity);
        lavaPS2 = Instantiate(lavaPS2Prefab, GameManager.Instance.player.transform.position, Quaternion.identity);
        lavaPS1.SetActive(true);
        lavaPS2.SetActive(true);
        yield return new WaitForSeconds(lavaExistTime);
        Destroy(lavaPS1);
        Destroy(lavaPS2);
        yield break;
    }

    protected override void ExitChaseState()
    {
        if (FoundPlayerInAttackRange() || (FoundPlayerInSkillRange() && skillCoolDown <= 0))
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

    public override void ExitAttackState()
    {
        if (!FoundPlayerInAttackRange() && !(FoundPlayerInSkillRange() && skillCoolDown <= 0))
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
