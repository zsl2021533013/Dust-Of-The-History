using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{

    public float g = 8.0f;

    public GameObject firePS;

    public GameObject explosionPS;

    public int damage = 6;

    public float damageRange = 3.0f;

    float speed = 0.0f;

    bool isFall = true;

    public bool FoundPlayerInExploseRange()
    {
        var colliders = Physics.OverlapSphere(transform.position, damageRange);

        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFall)
        {
            speed += g * Time.deltaTime;
            Vector3 pos = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            transform.position = pos;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            isFall = false;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            firePS.SetActive(false);
            StartCoroutine(Explose());

            if (FoundPlayerInExploseRange()) // 如果此时找到 player
            {
                GameManager.Instance.characterStats.TakeDamage(damage, GameManager.Instance.characterStats);
                if (!PlayerController.Instance.isKnockDown) // 判断是否已被击倒，避免起身后因 trigger 开启被再次击倒
                {
                    GameManager.Instance.player.GetComponent<Animator>().SetTrigger("Knockdown");
                }
            }
        }
    }

    IEnumerator Explose()
    {
        explosionPS.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        explosionPS.SetActive(false);
        Destroy(gameObject);
        yield break;
    }
}
