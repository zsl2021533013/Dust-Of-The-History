using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemUI))]
public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{

    SlotHolder currentHolder;

    SlotHolder targetHolder;

    ItemUI currentItemUI; // ��Ʒ���ݵĸ���

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
        transform.position = eventData.position; // ��Ʒ��������ƶ�
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (InventoryManager.Instance.CheckInUI(eventData.position))
            {
            
                if (eventData.pointerEnter.gameObject.GetComponent<SlotHolder>()) // ������� SlotHolder��ֱ�ӻ�ȡ
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponent<SlotHolder>();
                }
                else // �粻���ڣ���������ͼ��ĳ��֣�����ͼ��ĸ�����ȥѰ�� SlotHolder
                {
                    targetHolder = eventData.pointerEnter.gameObject.GetComponentInParent<SlotHolder>();
                }

                switch (targetHolder.slotType)
                {
                    case SlotType.BAG:
                        SwapItem();
                        break;
                    case SlotType.WEAPON: // ��������� weapon ���ܷ���
                        if(currentItemUI.Inventory.inventoryItems[currentItemUI.Index].itemData.itemType == ItemType.Weapon)
                        {
                            SwapItem();
                        }
                        break;
                    case SlotType.ARMOR: // ��������� armor ���ܷ���
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

    void SwapItem() // ������ǰ��Ʒ��Ŀ����Ʒ
    {
        var targetItem = targetHolder.itemUI.Inventory.inventoryItems[targetHolder.itemUI.Index]; // ��ȡ����
        var tempItem = currentHolder.itemUI.Inventory.inventoryItems[currentHolder.itemUI.Index];

        if(targetItem.itemData == tempItem.itemData && tempItem.itemData.stackable)
        {
            targetItem.amount += tempItem.amount;
            tempItem.itemData = null;
            tempItem.amount = 0; // �������
        }
        else // �����Ϊͬһ����
        {
            currentHolder.itemUI.Inventory.inventoryItems[currentHolder.itemUI.Index] = targetItem; 
            targetHolder.itemUI.Inventory.inventoryItems[targetHolder.itemUI.Index] = tempItem;
        } // TODO: �˴���Щ�ɻ�����Ǵ����ַ�Ļ���������д��Ӧ�������Ⱑ
    }
 
}
