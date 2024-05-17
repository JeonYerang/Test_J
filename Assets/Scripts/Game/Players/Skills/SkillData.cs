using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public string _name;

    public int damage;
    public int numOfShot;
    public float distance;
    public float range;

    //사용조건
    public int requiredAmount;

    //etc
    public Sprite icon;
    public string animationName;
    public GameObject skillPrefab;
}

public enum SkillConditionType
{
    Basic,
    CoolTime,
    Count
}

public class Skill
{
    PlayerAttack owner;

    public string _name;
    public SkillData data;

    public SkillConditionType conditionType;

    public Skill(PlayerAttack owner)
    {
        this.owner = owner;
    }

    public bool ConditionCheck(int amount)
    {
        return false;
    }

    public void UsingSkill()
    {
        
    }

    public void ShotSkill()
    {

    }
}