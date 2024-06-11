using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData : ScriptableObject
{
    public string _name;

    public int damage;
    public float coolTime;

    public Sprite icon;

    public string[] skillAnimation;
    public GameObject[] skillPrefab;
}
