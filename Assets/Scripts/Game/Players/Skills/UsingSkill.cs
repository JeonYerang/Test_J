using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsingSkill : MonoBehaviour
{
    SkillData skill;

    string _name;

    int damage;
    int numOfShot;
    float distance;
    float range;

    float chargeCount;
    float coolTime;

    Sprite icon;
    string animationName;
    GameObject skillPrefab;

    public Button skillButton;

    private void Awake()
    {
        skillButton.onClick.AddListener(OnClickSkillButton);
    }

    public void SetSkill(SkillData skill)
    {
        this.skill = skill;
        InitSkill();
        SetSkillIcon();
    }

    private void InitSkill()
    {
        _name = skill._name;

        damage = skill.damage;
        numOfShot = skill.numOfShot;
        distance = skill.distance;
        range = skill.range;

        chargeCount = skill.chargeCount;
        coolTime = skill.coolTime;

        icon = skill.icon;
        animationName = skill.animationName;
        skillPrefab = skill.skillPrefab;
    }

    private void SetSkillIcon()
    {
        skillButton.GetComponent<Image>().sprite = icon;
    }

    private void OnClickSkillButton()
    {
        UseSkill();
    }

    private void UseSkill()
    {

    }

    private void ShowCoolTime()
    {

    }
}
