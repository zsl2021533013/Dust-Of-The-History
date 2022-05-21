using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    Image healthSlider, expSlider;

    TMP_Text level;

    private void Awake()
    {
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        level = transform.GetChild(2).GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
        UpdateExpBar();
        UpdateLevel();
    }

    void UpdateHealthBar()
    {
        healthSlider.fillAmount = (float)GameManager.Instance.characterStats.CurrentHealth / (float)GameManager.Instance.characterStats.MaxHealth;
    }

    void UpdateExpBar()
    {
        expSlider.fillAmount = (float)GameManager.Instance.characterStats.CurrentExperiencePoint / (float)GameManager.Instance.characterStats.ExperienceThreshold;
    }

    void UpdateLevel()
    {
        level.text = "Level    " + GameManager.Instance.characterStats.CurrentLevel;
    }
}
