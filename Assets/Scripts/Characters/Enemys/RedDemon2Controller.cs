using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RedDemon2Controller : DemonController
{

    [Header("RedDemon Only")]
    public int skeletonAmount;

    public GameObject skeletonPrefab1;

    public GameObject skeletonPrefab2;

    public GameObject skeletonPrefab3;

    public GameObject wildFirePS;

    public GameObject rockPS;

    public GameObject ritualCirclePS;

    public float Attack3CoolDown;

    public float attack3Range;

    private float attack3CoolDown;

    Vector3 skeletonPos;

    public bool FoundPlayerInAttack3Range()
    {
        var colliders = Physics.OverlapSphere(transform.position, attack3Range);

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

    public void SummonSkeleton() // 制造骷髅兵
    {
        StartCoroutine(RitualCirclePS());
        for (int i = 0; i < skeletonAmount; i++)
        {
            float randomX = GameManager.Instance.player.transform.position.x + offsetFactor * (Random.value - 0.5f);
            float randomZ = GameManager.Instance.player.transform.position.z + offsetFactor * (Random.value - 0.5f);
            skeletonPos = new Vector3(randomX, GameManager.Instance.player.transform.position.y, randomZ);
            
            NavMeshHit hit;
            skeletonPos = NavMesh.SamplePosition(skeletonPos, out hit, characterStats.PatrolRange, 1) ? hit.position : Vector3.zero;

            if((skeletonPos == Vector3.zero)) // 如果位置不对就不生成
            {
                return;
            }

            int type = (int)Random.Range(1, 4);
            StartCoroutine(WildFirePS(type));
            
        }
    }

    IEnumerator RitualCirclePS()
    {
        GameObject tmp = 
            Instantiate(ritualCirclePS, GameManager.Instance.player.transform.position, Quaternion.identity);
        tmp.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        Destroy(tmp);
    }

    IEnumerator WildFirePS(int t)
    {
        yield return new WaitForSeconds(2.0f);
        GameObject tmp = Instantiate(wildFirePS, skeletonPos, Quaternion.identity);
        tmp.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        switch (t)
        {
            case 1:
                Instantiate(skeletonPrefab1, skeletonPos, Quaternion.identity);
                break;
            case 2:
                Instantiate(skeletonPrefab2, skeletonPos, Quaternion.identity);
                break;
            case 3:
                Instantiate(skeletonPrefab3, skeletonPos, Quaternion.identity);
                break;
            default:
                Instantiate(skeletonPrefab3, skeletonPos, Quaternion.identity);
                break;
        }
        yield return new WaitForSeconds(1.0f);
        Destroy(tmp);
    }

    public void Hit3()
    {
        StartCoroutine(RockPS());

        if (attackTarget == null) return;
        if (!FoundPlayerInAttack2Range()) return;

        var targetStats = attackTarget.GetComponent<CharacterStats>();
        characterStats.TakeDamage(characterStats, targetStats);

        attackTarget.GetComponent<Animator>().SetTrigger("Knockdown");
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

        if (attack3CoolDown > 0)
        {
            attack3CoolDown -= Time.deltaTime;
        }
        else if (FoundPlayerInAttack2Range())
        {
            transform.LookAt(attackTarget.transform);
            characterStats.isCritical = (Random.value < characterStats.CriticalChance);
            animator.SetTrigger("Attack3");
            attack3CoolDown = Attack3CoolDown;
        }
    }
}
