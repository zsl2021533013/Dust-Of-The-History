using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Useable,
    Weapon,
    Armor
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Data")]
public class ItemData_SO : ScriptableObject
{
    [Header("Base Information")]
    public ItemType itemType;

    public string itemName;

    public Sprite itemIcon;

    public int itemAmount;

    [TextArea]
    public string itemDescription;

    public bool stackable;

    [Space(10)]

    [Header("Weapon Information")]
    public GameObject weaponPrefab;

    public AttackData_SO attackData; // 作为武器时的攻击数值

    public AnimatorOverrideController overrideController;

    [Space(10)]

    [Header("Armor Information")]
    public GameObject armorPrefab;

    public CharacterData_SO characterData; // 作为武器时的攻击数值

    [Space(10)]

    [Header("Useable Item Information")]
    public UseableItemData_SO useableItemData;
}
