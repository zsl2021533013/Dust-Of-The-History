using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDrawController : MonoBehaviour
{
    public float minDamage;
    public float maxDamage;
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.name);

        if (other.CompareTag("Player"))
        {
            GameManager.Instance.characterStats.TakeDamage
                ((int)Random.Range(minDamage, maxDamage), GameManager.Instance.characterStats);
        }

    }
}
