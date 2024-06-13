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
    public SkillCastData castData;
}

[Serializable]
public class SkillCastData
{

}

[Serializable]
public class BasicCastData : SkillCastData
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}

[Serializable]
public class ComboCastData : SkillCastData
{
    public int maxComboCount;

    public string[] skillAnimation;
    public SkillObject[] skillPrefab;
}

[Serializable]
public class ChargeCastData : SkillCastData
{
    public int maxChargeCount;
    public float chargeInterval; //몇 초 마다 충전될 건지

    public string skillAnimation;
    public SkillObject[] skillPrefab;

    public string chargingAnimation;
    public GameObject ChargingPrefab;
}

[Serializable]
public class OnOffCastData : SkillCastData
{
    public string skillAnimation;
    public SkillObject skillPrefab;
}