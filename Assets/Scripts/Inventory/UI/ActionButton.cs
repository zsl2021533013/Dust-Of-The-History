using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;

    SlotHolder slotHolder;

    private void Awake()
    {
        slotHolder = GetComponent<SlotHolder>();
    }

    void Update()
    {
        if (Input.GetKeyDown(actionKey) && slotHolder.itemUI.GetItemData.itemData)
        {
            slotHolder.UseItem();
        }
    }
}
