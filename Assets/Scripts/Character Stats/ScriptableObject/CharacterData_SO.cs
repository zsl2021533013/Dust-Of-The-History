using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data",menuName = "Data/Character Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Character Information")]
    [Space(10)]

    [Header("Health Information")]
    public int MaxHealth;
    public int CurrentHealth;

    [Space(10)]

    [Header("Defence Information")]
    public int BaseDefence;
    public int CurrentDefence;

    [Space(10)]

    [Header("Experience Information")]
    public int MaxLevel;
    public int CurrentLevel;
    public int ExperienceThreshold; // 基础升一级所需经验
    public float LevelUpBuff; // 升级的提升百分比
    public int CurrentExperiencePoint;
    public int KillPoint;
}
