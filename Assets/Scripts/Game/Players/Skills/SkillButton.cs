using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    int num;
    Skill skill;

    SkillConditionType conditionType;
    int requiredAmount;

    Sprite icon;
    

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
        SkillManager.Instance.UsingSkill(num);
    }

    //condition
    public void ShowCoolTime()
    {

    }

    public void ShowRemainCount()
    {

    }
}
