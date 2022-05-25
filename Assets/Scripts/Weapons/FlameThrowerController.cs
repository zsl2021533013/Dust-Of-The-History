using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameThrowerController : MonoBehaviour
{
    public float minDamage;
    public float maxDamage;
    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.characterStats.TakeDamage
                ((int)Random.Range(minDamage, maxDamage), GameManager.Instance.characterStats);
        }
    }
}
