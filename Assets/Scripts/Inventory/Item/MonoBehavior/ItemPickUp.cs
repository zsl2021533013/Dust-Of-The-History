using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;

    [SerializeField]
    AudioClip m_audioClip;

    bool is_played = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pick up " + gameObject.name);

            InventoryManager.Instance.bagData.AddItem(itemData, itemData.itemAmount);

            InventoryManager.Instance.bagContainerUI.Refresh();

            //GameManager.Instance.characterStats.EquipWeapon(itemData); // ×°±¸ÎäÆ÷

            if (!is_played) BGMManager.Instance.PlayOneShot(m_audioClip);

            is_played = true;

            Destroy(gameObject);
        }
    }
}
