using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public bool alwaysVisible;

    public float visibleTime;

    public GameObject healthBar; // 获取预制件

    public Transform healthBarPos; // 获取生命条位置
    
    private CharacterStats characterStats; // 获取数据实例，改变生命条

    private new Transform camera; // 根据镜头使生命条转向

    private Transform m_healthBar; // 生成的生命条的的实例

    private Transform healthSilder; // 生命条的滑动条

    private float remainTime;

    private void OnEnable()
    {
        camera = Camera.main.transform;

        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                m_healthBar = Instantiate(healthBar, canvas.transform).transform;
                healthSilder = m_healthBar.GetChild(0);
                m_healthBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        characterStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if(characterStats.CurrentHealth <= 0)
        {
            Destroy(m_healthBar.gameObject);
        }
        m_healthBar.gameObject.SetActive(true);

        healthSilder.GetComponent<Image>().fillAmount = (float)currentHealth / (float)maxHealth;

        remainTime = visibleTime;
    }

    private void LateUpdate()
    {
        if(m_healthBar == null)
        {
            return;
        }
        m_healthBar.position = healthBarPos.position;
        m_healthBar.forward = camera.forward;

        if (remainTime < 0)
        {
            m_healthBar.gameObject.SetActive(false);
        }
        else if(!alwaysVisible)
        {
            remainTime -= Time.deltaTime;
        }
    }
}
