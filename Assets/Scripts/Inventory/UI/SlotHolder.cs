using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    BAG,
    WEAPON,
    ARMOR,
    ACTION
}

public class SlotHolder : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public SlotType slotType; // �������صķ��������
    public ItemUI itemUI; // �������صķ���

    private void OnDisable()
    {
        if (InventoryManager.Instance) // ע�⣬OnDisable ����Ϸ�˳�ʱ����ִ��һ�飬��ʱ InventoryManager ������
        {
            InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.clickCount > 1)
        {
            UseItem();
        }
    }

    public void UseItem() // ʹ�ø� SlotHolder �µ� item
    {

        if(itemUI.GetItemData.itemData && itemUI.GetItemData.itemData.itemType == ItemType.Useable && itemUI.GetItemData.amount > 0)
        {
            GameManager.Instance.characterStats.ApplyHealth(itemUI.GetItemData.itemData.useableItemData.HealthPoint);
            itemUI.GetItemData.amount--;
            UpdateItem();
        }
    }

    public void UpdateItem()
    {
        switch (slotType) 
        {
            case SlotType.BAG:
                itemUI.Inventory = InventoryManager.Instance.bagData; // ���ڱ����࣬���ñ������ݿ�
                break;
            case SlotType.WEAPON:
                itemUI.Inventory = InventoryManager.Instance.weaponData;
                if (itemUI.GetItemData != null && GameManager.Instance.characterStats)
                {
                    GameManager.Instance.characterStats.ChangeWeapon(itemUI.GetItemData.itemData);
                }
                else if(GameManager.Instance.characterStats) // ��ʱ���������δע��ʱ�����
                {
                    GameManager.Instance.characterStats.UnEquipWeapon();
                }
                break;
            case SlotType.ARMOR:
                itemUI.Inventory = InventoryManager.Instance.armorData;
                break;
            case SlotType.ACTION:
                itemUI.Inventory = InventoryManager.Instance.actionData;
                break;
        }

        var item = itemUI.Inventory.inventoryItems[itemUI.Index];
        itemUI.SetupItemUI(item.itemData, item.amount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemUI.GetItemData.itemData) // ������ݴ���
        {
            InventoryManager.Instance.itemTooltip.gameObject.SetActive(true);
            InventoryManager.Instance.itemTooltip.SetUpItemTooltip(itemUI.GetItemData.itemData);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryManager.Instance.itemTooltip.gameObject.SetActive(false);
    }
}
