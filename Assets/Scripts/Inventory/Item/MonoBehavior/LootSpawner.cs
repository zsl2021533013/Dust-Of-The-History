using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour
{
    [System.Serializable]
    public class LootItem
    {
        public GameObject item; // 掉落物品 
        
        [Range(0,1)]
        public float weight; // 掉落权重
    }

    public LootItem[] lootItems;

    public void SpawnLoop()
    {
        float currentWeight = Random.value;

        for(int i = 0; i < lootItems.Length; i++)
        {
            if(lootItems[i].weight > currentWeight)
            {
                Debug.Log(currentWeight);
                Debug.Log(lootItems[i].weight);
                GameObject tmp = Instantiate(lootItems[i].item);
                tmp.transform.position = transform.position + Vector3.up * 2.0f;
            }
        }
    }
}
