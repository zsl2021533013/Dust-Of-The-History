using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryController : MonoBehaviour
{
    const float AttackRange = 30.0f;

    public GameObject artilleryStrikePS;

    public Transform firePos;

    public GameObject bombPrefab;

    public GameObject ammoIcon;

    public float FireCoolDown;

    private float fireCoolDown;

    [SerializeField]
    AudioClip cannon;

    [SerializeField]
    Cinemachine.CinemachineImpulseSource cinemachineImpulseSource;

    public void FaceEnemyInAttackRange()
    {
        var colliders = Physics.OverlapSphere(transform.position, AttackRange);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                transform.LookAt(collider.gameObject.transform);
            }
        }
    }

    private void Update()
    {
        if(fireCoolDown > 0)
        {
            fireCoolDown -= Time.deltaTime;
        }
        else
        {
            ammoIcon.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(fireCoolDown > 0)
        {
            return;
        }

        if (other.gameObject.CompareTag("Player") && MouseManager.Instance.isClickArtillery)
        {
            FaceEnemyInAttackRange();
            StartCoroutine(Fire());
        }
    }

    public IEnumerator Fire()
    {
        fireCoolDown = FireCoolDown;

        ammoIcon.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        BGMManager.Instance.PlayOneShot(cannon);
        cinemachineImpulseSource.GenerateImpulse();

        artilleryStrikePS.SetActive(true);

        Instantiate(bombPrefab, firePos.position, firePos.rotation);

        yield return new WaitForSeconds(3.0f);

        artilleryStrikePS.SetActive(false);

        yield break;
    }
}
