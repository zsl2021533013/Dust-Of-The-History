using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateCharacterData; // 初始模板数据
    
    public AttackData_SO templateAttackData; // 初始模板数据

    [Space(10)]

    [Header("Optional Choice")]
    public RuntimeAnimatorController baseController;

    [HideInInspector]
    public CharacterData_SO characterData;

    [HideInInspector]
    public AttackData_SO attackData;

    public event Action<int, int> UpdateHealthBarOnAttack;

    [Space(10)]

    [Header("Player Only")]

    public GameObject levelUpFX;

    public CharacterData_SO templateBuffCharacterData; 

    public AttackData_SO templateBuffAttackData;

    public AttackData_SO templateWeaponAttackData;

    [HideInInspector]
    public CharacterData_SO buffCharacterData; // 记录已经使用的物品

    [HideInInspector]
    public AttackData_SO buffAttackData; // 记录已经使用的物品

    [HideInInspector]
    public AttackData_SO weaponAttackData;

    private void Awake()
    {
        InitializeData();
    }

    #region Read data

    public int MaxHealth { get { return characterData.MaxHealth; } set { characterData.MaxHealth = value; } }
    public int CurrentHealth { get { return characterData.CurrentHealth; } set { characterData.CurrentHealth = value;} }

    public int Defence { get { return characterData.Defence; } set { characterData.Defence = value; } }

    public int MaxLevel { get { return characterData.MaxLevel; } set { characterData.MaxLevel = value;} }
    public int CurrentLevel { get { return characterData.CurrentLevel; } set { characterData.CurrentLevel = value;} }
    public int ExperienceThreshold { get { return characterData.ExperienceThreshold; } set { characterData.ExperienceThreshold = value; } }
    public float LevelUpBuff { get { return characterData.LevelUpBuff; } set { characterData.LevelUpBuff = value;} }
    public int CurrentExperiencePoint { get { return characterData.CurrentExperiencePoint; } set { characterData.CurrentExperiencePoint = value; } }
    public int KillPoint { get { return characterData.KillPoint; } set { characterData.KillPoint = value;} }

    public float SightRange { get { return attackData.SightRange; } set { attackData.SightRange = value; } }

    public float PatrolSpeed { get { return attackData.PatrolSpeed; } set { attackData.PatrolSpeed = value; } }
    public float PatrolRange { get { return attackData.PatrolRange; } set { attackData.PatrolRange = value;} }
    public float PatrolCoolDown { get { return attackData.PatrolCoolDown; } set { attackData.PatrolCoolDown = value; } }

    public float ChaseSpeed { get { return attackData.ChaseSpeed; } set { attackData.ChaseSpeed = value; } }

    public float AttackRange { get { return attackData.AttackRange; } set { attackData.AttackRange = value; } }
    public float AttackCoolDown { get { return attackData.AttackCoolDown; } set { attackData.AttackCoolDown = value; } }

    public float SkillRange { get { return attackData.SkillRange; } set { attackData.SkillRange = value;} }
    public float SkillCoolDown { get { return attackData.SkillCoolDown; } set { attackData.SkillCoolDown = value;} }

    public int MinDamge { get { return attackData.MinDamge; } set { attackData.MinDamge = value; } }
    public int MaxDamge { get { return attackData.MaxDamge; } set { attackData.MaxDamge = value; } }

    public float CriticalMultiplier { get { return attackData.CriticalMultiplier; } set { attackData.CriticalMultiplier = value;} }
    public float CriticalChance { get { return attackData.CriticalChance; } set { attackData.CriticalChance = value;} }

    public Transform weaponPos; // 武器生成位置

    [HideInInspector]
    public bool isCritical;

    #endregion

    public void InitializeData() 
    {
        if (templateCharacterData != null)
        {
            characterData = Instantiate(templateCharacterData);
        }
        if (templateAttackData != null)
        {
            attackData = Instantiate(templateAttackData);
        }
        if (templateBuffCharacterData != null)
        {
            buffCharacterData = Instantiate(templateBuffCharacterData);
        }
        if (templateBuffAttackData != null)
        {
            buffAttackData = Instantiate(templateBuffAttackData);
        }
        if(templateWeaponAttackData != null)
        {
            weaponAttackData = Instantiate(templateWeaponAttackData);
        }
    }

    #region Character Combat

    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage(attacker, defener) - defener.Defence, 0);
        defener.CurrentHealth = Mathf.Max(defener.CurrentHealth - damage, 0);

        if(defener.CurrentHealth <= 0) // 提升经验
        {
            attacker.CurrentExperiencePoint += defener.KillPoint;
            if(attacker.CurrentExperiencePoint >= attacker.ExperienceThreshold)
            {
                attacker.LevelUp();
            }
        }

        defener.UpdateHealthBarOnAttack?.Invoke(defener.CurrentHealth, defener.MaxHealth);
    }

    public void TakeDamage(int damage, CharacterStats defener)
    {
        defener.CurrentHealth = Mathf.Max(defener.CurrentHealth - Mathf.Max(damage - defener.Defence, 0), 0);
        
        if (defener.CurrentHealth <= 0 && defener.CompareTag("Enemy")) // 提升经验
        {
            CharacterStats playerStats = GameManager.Instance.characterStats;
            playerStats.CurrentExperiencePoint += defener.KillPoint;
            if (playerStats.CurrentExperiencePoint >= playerStats.ExperienceThreshold)
            {
                playerStats.LevelUp();
            }
        }

        defener.UpdateHealthBarOnAttack?.Invoke(defener.CurrentHealth, defener.MaxHealth);
    }

    private int CurrentDamage(CharacterStats attacker, CharacterStats defener)
    {
        float coreDamage = UnityEngine.Random.Range(attacker.MinDamge, attacker.MaxDamge);
        if (attacker.isCritical)
        {
            coreDamage *= attacker.CriticalMultiplier;
            defener.GetComponent<Animator>().SetTrigger("Hit");
        }
        return (int)(coreDamage);
    }

    public void UpdatePlayerData()
    {
        MaxHealth = 100 + (CurrentLevel - 1) * 15;

        AttackCoolDown = weaponAttackData.AttackCoolDown != 0 ?
             weaponAttackData.AttackCoolDown : templateAttackData.AttackCoolDown;
        AttackRange = weaponAttackData.AttackRange != 0 ?
             weaponAttackData.AttackRange : templateAttackData.AttackRange;

        MinDamge = templateAttackData.MinDamge + buffAttackData.MinDamge 
            + weaponAttackData.MinDamge + CurrentLevel - 1;
        MaxDamge = templateAttackData.MaxDamge + buffAttackData.MaxDamge 
            + weaponAttackData.MaxDamge + CurrentLevel - 1;
        CriticalChance = templateAttackData.CriticalChance 
            + buffAttackData.CriticalChance + weaponAttackData.CriticalChance;
        Defence = templateCharacterData.Defence + buffCharacterData.Defence + (CurrentLevel - 1) / 2;
    }

    void LevelUp() // 等级提升
    {
        Debug.Log("Level Up");

        StartCoroutine(LevelUpFX());

        float multiplier = 1 + (CurrentLevel - 1) * LevelUpBuff;

        CurrentExperiencePoint -= ExperienceThreshold;
        ExperienceThreshold = (int)(multiplier * ExperienceThreshold);
        
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        if (CurrentLevel == MaxLevel) return;

        UpdatePlayerData();
        CurrentHealth = MaxHealth;

    }

    IEnumerator LevelUpFX()
    {
        levelUpFX.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        levelUpFX.SetActive(false);
        yield break;
    }

    #endregion

    #region Equip Weapons

    public void EquipWeapon(ItemData_SO itemData)
    {
        if(itemData == null) return;

        Debug.Log("Equiped Weapon");

        Instantiate(itemData.weaponPrefab, weaponPos); // 先生成武器

        GameManager.Instance.characterStats.ApplyAttackData(itemData.attackData); // 再更新玩家数值

        GetComponent<Animator>().runtimeAnimatorController = itemData.overrideController;
    }

    public void UnEquipWeapon()
    {
        Debug.Log("Remove Equipment");
        if(weaponPos.childCount > 0)
        {
            for (int i = 0; i < weaponPos.childCount; i++)
            {
                Destroy(weaponPos.GetChild(i).gameObject);
            }

            weaponAttackData = Instantiate(templateWeaponAttackData);
            UpdatePlayerData();

            GetComponent<Animator>().runtimeAnimatorController = baseController;
        }
        
    }

    public void ChangeWeapon(ItemData_SO itemData)
    {
        UnEquipWeapon();
        EquipWeapon(itemData);
    }

    public void ApplyAttackData(AttackData_SO itemAttackData) //将攻击数据传入玩家数据
    {
        weaponAttackData = itemAttackData;

        UpdatePlayerData();

        Debug.Log("Player's attack data has been updated");
    }

    #endregion

    #region 使用物品并进行数值修改

    public void ApplyHealth(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    }

    public void ApplyDefence(int amount)
    {
        buffCharacterData.Defence += amount;
    }

    public void ApplyAttack(int amount)
    {
        buffAttackData.MinDamge += amount;
        buffAttackData.MaxDamge += amount;
    }

    public void ApplyCritical(float amount)
    {
        buffAttackData.CriticalChance += amount;
    }

    public void ApplyItem(UseableItemData_SO useableItemData)
    {
        ApplyHealth(useableItemData.HealthPoint);
        ApplyDefence(useableItemData.DefencePoint);
        ApplyAttack(useableItemData.AttackPoint);
        ApplyCritical(useableItemData.CriticalPoint);
        UpdatePlayerData();
    }

    #endregion
}
