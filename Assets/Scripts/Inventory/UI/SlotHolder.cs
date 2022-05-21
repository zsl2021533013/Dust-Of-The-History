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
    public SlotType slotType; // 他所承载的方格的类型
    public ItemUI itemUI; // 他所承载的方格

    private void OnDisable()
    {
        if (InventoryManager.Instance) // 注意，OnDisable 在游戏退出时会再执行一遍，此时 InventoryManager 不存在
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

    public void UseItem() // 使用该 SlotHolder 下的 item
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
                itemUI.Inventory = InventoryManager.Instance.bagData; // 属于背包类，调用背包数据库
                break;
            case SlotType.WEAPON:
                itemUI.Inventory = InventoryManager.Instance.weaponData;
                if (itemUI.GetItemData != null && GameManager.Instance.characterStats)
                {
                    GameManager.Instance.characterStats.ChangeWeapon(itemUI.GetItemData.itemData);
                }
                else if(GameManager.Instance.characterStats) // 有时会在玩家尚未注册时便调用
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
        if (itemUI.GetItemData.itemData) // 如果数据存在
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
