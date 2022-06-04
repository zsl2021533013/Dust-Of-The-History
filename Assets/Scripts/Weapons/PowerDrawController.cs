using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDrawController : MonoBehaviour
{
    public float minDamage;
    public float maxDamage;

    public float rotSpeed;

    void Update()
    {
        Transform targetPos = GameManager.Instance.player.transform;
        Quaternion targetRot = Quaternion.LookRotation(targetPos.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * rotSpeed);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.characterStats.TakeDamage
                ((int)Random.Range(minDamage, maxDamage), GameManager.Instance.characterStats);
        }
    }
}
