using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public string _name;

    //수치정보
    public int damage;
    public int numOfShot;
    public float distance;
    public float range;

    //사용조건
    public float chargeCount;
    public float coolTime;

    //etc
    public Sprite icon;
    public string animationName;
    public GameObject skillPrefab;
}

public class Skill
{
    public SkillData data;

    public void ConditionCheck()
    {

    }

    public void UsingSkill()
    {

    }
}