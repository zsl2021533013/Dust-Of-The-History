using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public bool alwaysVisible;

    public float visibleTime;

    public GameObject healthBar; // ��ȡԤ�Ƽ�

    public Transform healthBarPos; // ��ȡ������λ��
    
    private CharacterStats characterStats; // ��ȡ����ʵ�����ı�������

    private new Transform camera; // ���ݾ�ͷʹ������ת��

    private Transform m_healthBar; // ���ɵ��������ĵ�ʵ��

    private Transform healthSilder; // �������Ļ�����

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
