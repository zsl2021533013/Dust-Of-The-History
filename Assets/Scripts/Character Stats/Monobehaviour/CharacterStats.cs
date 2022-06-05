using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public CharacterData_SO templateCharacterData; // 模板数据
    
    public AttackData_SO templateAttackData; // 模板数据

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

    private void Awake()
    {
        if(templateCharacterData != null)
        {
            characterData = Instantiate(templateCharacterData);
        }  
        if(templateAttackData != null)
        {
            attackData = Instantiate(templateAttackData);
        }
    }

    #region Read data

    public int MaxHealth { get { return characterData.MaxHealth; } set { characterData.MaxHealth = value; } }
    public int CurrentHealth { get { return characterData.CurrentHealth; } set { characterData.CurrentHealth = value;} }

    public int BaseDefence { get { return characterData.BaseDefence; } set { characterData.BaseDefence = value; } }
    public int CurrentDefence { get { return characterData.CurrentDefence; } set { characterData.CurrentDefence = value;} }

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

    public Transform armorPos; // 盔甲生成位置

    [HideInInspector]
    public bool isCritical;

    #endregion

    #region Character combat

    public void TakeDamage(CharacterStats attacker, CharacterStats defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage(attacker, defener) - defener.CurrentDefence, 0);
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
        defener.CurrentHealth = Mathf.Max(defener.CurrentHealth - Mathf.Max(damage - defener.CurrentDefence, 0), 0);
        
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

    void LevelUp() // 等级提升
    {
        Debug.Log("Level Up");

        StartCoroutine(LevelUpFX());

        float multiplier = 1 + (CurrentLevel - 1) * LevelUpBuff;

        CurrentExperiencePoint -= ExperienceThreshold;
        ExperienceThreshold = (int)(multiplier * ExperienceThreshold);
        
        CurrentLevel = Mathf.Clamp(CurrentLevel + 1, 0, MaxLevel);

        if (CurrentLevel == MaxLevel) return;

        MaxHealth = (int)(multiplier * MaxHealth);
        CurrentHealth = MaxHealth;

        MinDamge = templateAttackData.MinDamge + 2 * CurrentLevel;
        MaxDamge = templateAttackData.MaxDamge + 2 * CurrentLevel;

        CurrentDefence = templateCharacterData.BaseDefence + CurrentLevel;
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
            attackData = Instantiate(templateAttackData); // 还原数据
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
        AttackRange = itemAttackData.AttackRange;
        AttackCoolDown = itemAttackData.AttackCoolDown;
        MinDamge = templateAttackData.MinDamge + itemAttackData.MinDamge;
        MaxDamge = templateAttackData.MaxDamge + itemAttackData.MaxDamge;
        CriticalMultiplier = itemAttackData.CriticalMultiplier;
        CriticalChance = itemAttackData.CriticalChance;

        Debug.Log("Player's attack data has been updated");
    }

    #endregion

    #region 切换装备

    public void EquipArmor(ItemData_SO itemData)
    {
        if (itemData == null) return;

        Instantiate(itemData.weaponPrefab, armorPos); // 先生成武器

        GameManager.Instance.characterStats.ApplyAttackData(itemData.attackData); // 再更新玩家数值

        GetComponent<Animator>().runtimeAnimatorController = itemData.overrideController;
    }

    public void UnEquipArmor()
    {
        Debug.Log("Remove Equipment");
        if (weaponPos.childCount > 0)
        {
            for (int i = 0; i < weaponPos.childCount; i++) // TODO: 需要更改。我们可以直接 SetActive 开控制，不必生成
            {
                Destroy(weaponPos.GetChild(i).gameObject);
            }
            characterData = Instantiate(templateCharacterData); // 还原数据
            GetComponent<Animator>().runtimeAnimatorController = baseController;
        }

    }

    public void ChangeArmor(ItemData_SO itemData)
    {
        UnEquipArmor();
        EquipArmor(itemData);
    }

    public void ApplyCharacterData(CharacterData_SO itemCharacterData) //将角色数据传入玩家数据
    {
        MaxHealth = templateCharacterData.MaxHealth + itemCharacterData.MaxHealth;

        CurrentDefence = templateCharacterData.BaseDefence + itemCharacterData.BaseDefence;

        Debug.Log("Player's character data has been updated");
    }

    #endregion

    #region 使用物品并进行数值修改

    public void ApplyHealth(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    }

    public void ApplyDefence(int amount)
    {
        CurrentDefence += amount;
    }

    public void ApplyAttack(int amount)
    {
        MinDamge += amount;
        MaxDamge += amount;
    }

    public void ApplyCritical(float amount)
    {
        CriticalChance += amount;
    }

    public void ApplyItem(UseableItemData_SO useableItemData)
    {
        ApplyHealth(useableItemData.HealthPoint);
        ApplyDefence(useableItemData.DefencePoint);
        ApplyAttack(useableItemData.AttackPoint);
        ApplyCritical(useableItemData.CriticalPoint);
    }

    #endregion
}
