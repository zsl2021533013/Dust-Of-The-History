using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Data", menuName = "Data/Inventory Data")]
public class InventoryData_SO : ScriptableObject
{
    public List<InventoryItem> inventoryItems;

    public void AddItem(ItemData_SO itemData,int amount)
    {
        bool found = false; // 是否在背包中找到物品
        if (itemData.stackable) // 可以被堆叠
        {
            foreach(var item in inventoryItems)
            {
                if(item.itemData == itemData) // 找到相同的物品
                {
                    Debug.Log(itemData.name + " has added");
                    item.amount += amount;
                    found = true;
                    break;
                }
            }
        }
        if (!found) // 如果没有找到，或是不能堆叠
        {
            for (int i = 0; i < inventoryItems.Count; i++) // 寻找最近空格
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
