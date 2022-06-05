using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Useable Item Data", menuName = "Data/Useable Item Data")]
public class UseableItemData_SO : ScriptableObject
{
    public int HealthPoint;
    public int DefencePoint;
    public int AttackPoint;
    public float CriticalPoint;
}
