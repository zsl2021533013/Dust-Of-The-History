using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public Image image = null;

    public TMP_Text amount = null;

    public InventoryData_SO Inventory { get; set; } // ����Ʒ�����������ݿ⣬���ڻ�ȡ����

    public int Index { get; set; } = -1; // ����Ʒ�ڸ����ݿ��λ�ñ��

    public InventoryItem GetItemData { get { return Inventory.inventoryItems[Index]; } set { Inventory.inventoryItems[Index] = value; } }

    public void SetupItemUI(ItemData_SO itemData, int itemAmount) // ����Ͻ����Ʒ���ݴ���
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
