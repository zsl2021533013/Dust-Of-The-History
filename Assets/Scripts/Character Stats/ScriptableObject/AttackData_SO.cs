using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack Data", menuName = "Data/Attack Data")]
public class AttackData_SO : ScriptableObject
{
    [Header("Attack Information")]

    [Space(10)]

    [Header("Sight Information")]
    public float SightRange;

    [Space(10)]

    [Header("Patrol Information")]
    public float PatrolSpeed;
    public float PatrolRange;
    public float PatrolCoolDown;

    [Space(10)]

    [Header("Chase Information")]
    public float ChaseSpeed;

    [Space(10)]

    [Header("Attack Information")]
    public float AttackRange;
    public float AttackCoolDown;

    [Space(10)]

    [Header("Skill Information")]
    public float SkillRange;
    public float SkillCoolDown;

    [Space(10)]

    [Header("Damge Information")]
    public int MinDamge;
    public int MaxDamge;

    [Space(10)]

    [Header("Critical Information")]
    public float CriticalMultiplier;
    public float CriticalChance;
}
