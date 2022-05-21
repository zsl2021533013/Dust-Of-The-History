using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Data", menuName = "Data/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> inventoryItems;

    public void AddItem(ItemData_SO itemData,int amount)
    {
        bool found = false; // �Ƿ��ڱ������ҵ���Ʒ
        if (itemData.stackable) // ���Ա��ѵ�
        {
            foreach(var item in inventoryItems)
            {
                if(item.itemData == itemData) // �ҵ���ͬ����Ʒ
                {
                    Debug.Log(itemData.name + " has added");
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }
        if (!found) // ���û���ҵ������ǲ��ܶѵ�
        {
            for (int i = 0; i < inventoryItems.Count; i++) // Ѱ������ո�
            {
                if (inventoryItems[i].itemData == null)
                {
                    Debug.Log(itemData.name + " has added");
                    inventoryItems[i].itemData = itemData;
                    inventoryItems[i].amount = amount;
                    break;
                }
            }
        }
    }
}

[System.Serializable]
public class InventoryItem 
{
    public ItemData_SO itemData;
    public int amount;
}
