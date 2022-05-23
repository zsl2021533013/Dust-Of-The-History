using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MachanicalGolem : EnemyController
{
    public int minFlameDamage;

    public int maxFlameDamage;

    public int minShootDamage;
    
    public int maxShootDamage;

    public float dotThreshold;

    public GameObject slashPS1;

    public GameObject slashPS2;

    public override void Hit()
    {
        StartCoroutine(SlashPS1());
    }

    public void SkillHit()
    {
        StartCoroutine(SlashPS2());
    }

    IEnumerator SlashPS1()
    { 
        StartCoroutine("BurnPlayer");
        slashPS1.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        StopCoroutine("BurnPlayer");
        slashPS1.SetActive(false);
        yield break;
    }

    IEnumerator SlashPS2()
    {
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("ShootPlayer");
        slashPS2.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        StopCoroutine("ShootPlayer");
        slashPS2.SetActive(false);
        yield break;
    }

    IEnumerator BurnPlayer()
    {
        while (true)
        {
            Debug.Log(1);

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
            Debug.Log(2);

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
}
