using Photon.Pun;
using Photon.Realtime;
using System;
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

    [SerializeField]
    ClassData[] classDataList;

    private void Awake()
    {
        Instance = this;
    }

/*    public void GetClassList()
    {
        foreach (PlayerClass playerClass in Enum.GetValues(typeof(PlayerClass)))
        {
            GameObject classEntry = Instantiate(classTogglePrefab, classSelectParent);

            InitClassToggle(classEntry, playerClass);
        }
    }*/

    public void SetClass()
    {
        Player player = PhotonNetwork.LocalPlayer;
        PlayerClass playerClass = (PlayerClass)((int)player.CustomProperties["Class"]);
        SkillData[] skillSets = ClassManager.Instance.GetSkillSets(playerClass);
        SkillManager.Instance.InitSkillUI(skillSets);
    }

    public PlayerClass GetClass()
    {
        Player player = PhotonNetwork.LocalPlayer;
        PlayerClass playerClass = (PlayerClass)((int)player.CustomProperties["Class"]);

        return playerClass;
    }

    public ClassData GetClassData(PlayerClass playerClass)
    {
        return classDataList[(int)playerClass];
    }
    public ClassData GetClassData(int playerClass)
    {
        return classDataList[playerClass];
    }

    public SkillData[] GetSkillSets(PlayerClass playerClass)
    {
        ClassData classData = classDataList[(int)playerClass];
        return classData.skills;
    }
}
