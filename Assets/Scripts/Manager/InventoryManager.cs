using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : Singleton<InventoryManager>
{
    public class DragData
    {
        public SlotHolder originalHolder;
        public RectTransform originalParent;
    }

    [Header("Slot Information")]
    public float slotGap; // �������ݿ�

    [Space(10)]

    [Header("Inventory Data")]
    public InventoryData_SO templateBagData; // �������ݿ�

    public InventoryData_SO templateActionData; // ��������ݿ�

    public InventoryData_SO templateWeaponData; // �������ݿ�

    public InventoryData_SO templateArmorData; // װ�����ݿ�

    [HideInInspector]
    public InventoryData_SO bagData; // �������ݿ�

    [HideInInspector]
    public InventoryData_SO actionData; // ��������ݿ�

    [HideInInspector]
    public InventoryData_SO weaponData; // �������ݿ�

    [HideInInspector]
    public InventoryData_SO armorData; // �������ݿ�

    [Space(10)]

    [Header("Bag Container")]
    public ContainerUI bagContainerUI;

    public ContainerUI actionContainerUI;

    public ContainerUI weaponContainerUI;

    public ContainerUI armorContainerUI;

    [Space(10)]

    [Header("Drag Canvas")]
    public Canvas dragCanvas;

    public DragData currentDrag;

    [Space(10)]

    [Header("UI Panel")]
    public GameObject characterStatsPanel;

    public GameObject bagPanel;

    private bool isOpen = false;

    [Space(10)]

    [Header("UI Text")]
    public TMP_Text healthText;

    public TMP_Text attackText;

    public TMP_Text defenceText;

    [Space(10)]

    [Header("Item Tooltip")]
    public ItemTooltip itemTooltip;

    protected override void Awake()
    {
        base.Awake();
        if(templateBagData != null)
        {
            bagData = Instantiate(templateBagData);
        }
        if (templateActionData != null)
        {
            actionData = Instantiate(templateActionData);
        }
        if (templateWeaponData != null)
        {
            weaponData = Instantiate(templateWeaponData);
        }
        if (templateArmorData != null)
        {
            armorData = Instantiate(templateArmorData);
        }
    }
    private void Start()
    {
        bagContainerUI.Refresh();
        actionContainerUI.Refresh();
        weaponContainerUI.Refresh();
        armorContainerUI.Refresh();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isOpen = !isOpen;
            characterStatsPanel.SetActive(isOpen);
            bagPanel.SetActive(isOpen);
        }

        UpdataCharacterText();
    }

    public void SaveData()
    {
        SaveManager.Instance.Save(bagData, bagData.name);
        SaveManager.Instance.Save(actionData, actionData.name);
        SaveManager.Instance.Save(weaponData, weaponData.name);
        SaveManager.Instance.Save(armorData, armorData.name);
    }

    public void LoadData()
    {
        SaveManager.Instance.Load(bagData, bagData.name);
        SaveManager.Instance.Load(actionData, actionData.name);
        SaveManager.Instance.Load(weaponData, weaponData.name);
        SaveManager.Instance.Load(armorData, armorData.name);
    }

    void UpdataCharacterText()
    {
        healthText.text = "Health : " + GameManager.Instance.characterStats.CurrentHealth;
        attackText.text = "ATK : " + GameManager.Instance.characterStats.MinDamge + "- " + GameManager.Instance.characterStats.MaxDamge;
        defenceText.text = "Defence : " + GameManager.Instance.characterStats.CurrentDefence;
    }

    #region ��Ʒ��ק������Ƿ��� UI �� 

    public bool CheckInBagUI(Vector3 pos)
    {
        for(int i = 0;i < bagContainerUI.slotHolders.Length; i++) // ѭ�����з���
        {
            RectTransform rectTransform = bagContainerUI.slotHolders[i].transform as RectTransform;
            if(RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInActionUI(Vector3 pos)
    {
        for (int i = 0; i < actionContainerUI.slotHolders.Length; i++) // ѭ�����з���
        {
            RectTransform rectTransform = actionContainerUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInWeaponUI(Vector3 pos)
    {
        for (int i = 0; i < weaponContainerUI.slotHolders.Length; i++) // ѭ�����з���
        {
            RectTransform rectTransform = weaponContainerUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInArmorUI(Vector3 pos)
    {
        for (int i = 0; i < weaponContainerUI.slotHolders.Length; i++) // ѭ�����з���
        {
            RectTransform rectTransform = weaponContainerUI.slotHolders[i].transform as RectTransform;
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, pos))
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckInUI(Vector3 pos) // ����λ���Ƿ������Ʒ
    {
        return CheckInBagUI(pos) || CheckInActionUI(pos) || CheckInWeaponUI(pos) || CheckInArmorUI(pos);
    }

    #endregion
}
