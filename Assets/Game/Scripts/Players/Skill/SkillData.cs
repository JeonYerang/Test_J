using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class SkillData : ScriptableObject
{
    public string _name;
    public Sprite icon;

    public int damage;
    public float coolTime;

    [HideInInspector]
    public SkillCastType castType;

    [SerializeReference]
    public ISkillTypeParam typeParam;
}

public interface ISkillTypeParam { }

[Serializable]
public class BasicTypeParam : ISkillTypeParam
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}

[Serializable]
public class ComboTypeParam : ISkillTypeParam
{
    public int maxComboCount;

    public string[] skillAnimations;
    public SkillObject[] skillPrefabs;
}

[Serializable]
public class ChargeTypeParam : ISkillTypeParam
{
    public int maxChargeCount;
    public float chargeInterval; //몇 초 마다 충전될 건지

    public string skillAnimation;
    public SkillObject[] skillPrefabs;

    public string chargingAnimation;
    public GameObject ChargingPrefab;
}

[Serializable]
public class OnOffTypeParam : ISkillTypeParam
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}