using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{

    SlotHolder currentHolder;

    SlotHolder targetHolder;

    ItemUI currentItemUI; // 物品数据的复制

    void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
        currentHolder = GetComponentInParent<SlotHolder>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        InventoryManager.Instance.currentDrag = new InventoryManager.DragData();
        InventoryManager.Instance.currentDrag.originalHolder = GetComponent<SlotHolder>();
        InventoryManager.Instance.currentDrag.originalParent = (RectTransform)transform.parent;

        transform.SetParent(InventoryManager.Instance.dragCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; // 物品跟随鼠标移动
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInUI(eventData.position))
            {
            
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>()) // 如果存在 SlotHolder，直接获取
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else // 如不存在，则是由于图像的出现，则在图像的父级中去寻找 SlotHolder
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                switch (targetHolder.slotType)
                {
                    case SlotType.BAG:
                        SwapItem();
                        break;
                    case SlotType.WEAPON: // 如果类型是 weapon 才能放入
                        if(currentItemUI.Inventory.inventoryItems[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.ARMOR: // 如果类型是 armor 才能放入
                        if (currentItemUI.Inventory.inventoryItems[currentItemUI.Index].itemData.itemType == ItemType.Armor)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.ACTION:
                        if (currentItemUI.Inventory.inventoryItems[currentItemUI.Index].itemData.itemType == ItemType.Useable)
                        {
                            SwapItem();
                        }
                        break;
                }

                currentHolder.UpdateItem();
                targetHolder.UpdateItem();
            }

            transform.SetParent(InventoryManager.Instance.currentDrag.originalParent);

            RectTransform t = transform as RectTransform;
            t.offsetMin = Vector2.one * InventoryManager.Instance.slotGap;
            t.offsetMax = -Vector2.one * InventoryManager.Instance.slotGap;
        }
        
    }

    void SwapItem() // 交换当前物品与目标物品
    {
        var targetItem = targetHolder.itemUI.Inventory.inventoryItems[targetHolder.itemUI.Index]; // 获取数据
        var tempItem = currentHolder.itemUI.Inventory.inventoryItems[currentHolder.itemUI.Index];

        if(targetItem.itemData == tempItem.itemData && tempItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0; // 数据清空
        }
        else // 如果不为同一类型
        {
            currentHolder.itemUI.Inventory.inventoryItems[currentHolder.itemUI.Index] = targetItem; 
            targetHolder.itemUI.Inventory.inventoryItems[targetHolder.itemUI.Index] = tempItem;
        } // TODO: 此处有些疑惑，如果是传入地址的话，这样的写法应该有问题啊
    }
 
}
