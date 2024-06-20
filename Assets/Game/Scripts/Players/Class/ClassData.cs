using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Class Data", menuName = "Scriptable Object/Class Data")]
public class ClassData : ScriptableObject
{
    public string className;
    public string classDescription;

    public Sprite classIcon;
    public GameObject weaponPrefab;
    public SkillData[] skills;
}
