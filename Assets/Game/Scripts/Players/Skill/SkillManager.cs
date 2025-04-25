using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
public struct SkillPair
{
    public SkillCastType skillCastType;
    public Skill skill;
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    [SerializeField]
    SkillPair[] skillPairList; //인스펙터로 스킬 딕셔너리 초기화용

    private Dictionary<SkillCastType, Skill> skillCreateDic
        = new Dictionary<SkillCastType, Skill>();

    [SerializeField]
    ButtonBinder skillButtonsUI;

    private void Awake()
    {
        Instance = this;
        InitSkillCreateDic();
    }

    #region 스킬 리스트 초기화
    private void InitSkillCreateDic()
    {
        foreach (var skillPair in skillPairList)
        {
            if(skillPair.skill != null)
                skillCreateDic.Add(skillPair.skillCastType, skillPair.skill);
        }
    }

    public void InitSkillUI(SkillData[] skillDataList)
    {
        skillButtonsUI.InitSkillButtons(skillDataList);
    }

    public Skill[] GetSkillList(SkillData[] skillDataList)
    {
        Skill[] skillList = new Skill[skillDataList.Length];

        for (int i = 0; i < skillDataList.Length; i++)
        {
            Skill newSkill = Instantiate(skillCreateDic[skillDataList[i].castType]);
            newSkill.Init(skillDataList[i]);
            skillList[i] = newSkill;
        }

        return skillList;
    }
    #endregion
}