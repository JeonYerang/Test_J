using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using System;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

[CreateAssetMenu(fileName = "Skill Set", menuName = "Scriptable Object/Skill Set")]
public class SkillSet : ScriptableObject
{
    public string _name;
    public Sprite icon;

    public int damage;
    public float coolTime;

    [HideInInspector]
    public SkillCastType castType;

    [SerializeReference]
    public ISkillCastData castData;
}

public interface ISkillCastData { }

[Serializable]
public class BasicCastData : ISkillCastData
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}

[Serializable]
public class ComboCastData : ISkillCastData
{
    public int maxComboCount;

    public string[] skillAnimation;
    public SkillObject[] skillPrefab;
}

[Serializable]
public class ChargeCastData : ISkillCastData
{
    public int maxChargeCount;
    public float chargeInterval; //몇 초 마다 충전될 건지

    public string skillAnimation;
    public SkillObject[] skillPrefab;

    public string chargingAnimation;
    public GameObject ChargingPrefab;
}

[Serializable]
public class OnOffCastData : ISkillCastData
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}