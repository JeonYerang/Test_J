using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Warrior,
    Archer,
    Tanker,
    Healer
}

public class ClassManager : MonoBehaviour
{
    public static ClassManager Instance { get; private set; }

    public ClassData[] classList;

    private void Awake()
    {
        Instance = this;
    }

    public ClassData GetClassData(PlayerClass playerClass)
    {
        return classList[(int)playerClass];
    }

    public SkillData[] GetSkillSets(PlayerClass playerClass)
    {
        ClassData classData = classList[(int)playerClass];
        return classData.skills;
    }
}
