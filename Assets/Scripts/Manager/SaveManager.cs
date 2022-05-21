using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{ 
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.S))
        {
            SavePlayerData();
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            LoadPlayerData();
        }
    }

    public void SavePlayerData()
    {
        if(GameManager.Instance.characterStats != null)
        {
            Save(GameManager.Instance.characterStats.characterData, "PlayerCharacterData");
            Save(GameManager.Instance.characterStats.attackData, "PlayerAttackData");
            PlayerPrefs.SetString("CurrentSceneName", SceneManager.GetActiveScene().name); // ���µ�ǰ����λ�ã�Ϊ continue ���泡����

            Debug.Log("Player's data has been saved");

            if (InventoryManager.Instance) // InventoryManager һ��ʼ�ǹرյ�
            {
                InventoryManager.Instance.SaveData();
            }
        }
    }

    public void LoadPlayerData()
    {
        if(GameManager.Instance.characterStats != null)
        {
            Load(GameManager.Instance.characterStats.characterData, "PlayerCharacterData");
            Load(GameManager.Instance.characterStats.attackData, "PlayerAttackData");

            Debug.Log("Player's data has been loaded");

            InventoryManager.Instance.LoadData();
        }
    }

    public void Save(object data,string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key, jsonData);
        PlayerPrefs.Save();
    }

    public void Load(object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key), data);
        }
    }
}
