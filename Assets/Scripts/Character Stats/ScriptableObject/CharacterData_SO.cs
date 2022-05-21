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
    public int ExperienceThreshold; // ������һ�����辭��
    public float LevelUpBuff; // �����������ٷֱ�
    public int CurrentExperiencePoint;
    public int KillPoint;
}
