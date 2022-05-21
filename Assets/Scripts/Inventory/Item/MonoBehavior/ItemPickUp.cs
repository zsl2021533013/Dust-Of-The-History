using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(SphereCollider))]
public class ItemPickUp : MonoBehaviour
{
    public ItemData_SO itemData;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Pick up " + gameObject.name);

            InventoryManager.Instance.bagData.AddItem(itemData, itemData.itemAmount);

            InventoryManager.Instance.bagContainerUI.Refresh();

            //GameManager.Instance.characterStats.EquipWeapon(itemData); // ×°±¸ÎäÆ÷

            Destroy(gameObject);
        }
    }
}
