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

    //etc
    public Sprite icon;
    public string animationName;
    public GameObject skillPrefab;
}