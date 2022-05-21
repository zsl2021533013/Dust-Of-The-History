using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image image = null;

    public TMP_Text amount = null;

    public InventoryData_SO Inventory { get; set; } // 该物品所从属的数据库，用于获取数据

    public int Index { get; set; } = -1; // 该物品在该数据库的位置编号

    public InventoryItem GetItemData { get { return Inventory.inventoryItems[Index]; } set { Inventory.inventoryItems[Index] = value; } }

    public void SetupItemUI(ItemData_SO itemData, int itemAmount) // 将下辖的物品数据传入
    {
        if(GetItemData.amount == 0)
        {
            GetItemData.itemData = null;
            image.gameObject.SetActive(false);
            return;
        }

        if(itemData == null)
        {
            image.gameObject.SetActive(false);
            return;
        }

        image.sprite = itemData.itemIcon;
        amount.text = itemAmount.ToString();
        image.gameObject.SetActive(true);
    }

}
