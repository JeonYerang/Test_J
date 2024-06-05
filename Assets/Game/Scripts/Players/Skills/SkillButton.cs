using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    int num;
    Skill skill;

    Sprite icon;
    Image coolIndicator;

    public Button skillButton;

    private void Awake()
    {
        skillButton.onClick.AddListener(OnClickSkillButton);
    }

    public void SetSkill(Skill skill)
    {
        this.skill = skill;
        this.icon = skill.data.icon;
        SetSkillIcon();
    }

    private void SetSkillIcon()
    {
        skillButton.GetComponent<Image>().sprite = icon;
    }

    private void OnClickSkillButton()
    {
        
    }

    //condition
    public void ShowCoolTime(float amount)
    {
        coolIndicator.fillAmount = amount;
    }

    public void ShowRemainCount(float amount)
    {
        coolIndicator.fillAmount = amount;
    }
}
