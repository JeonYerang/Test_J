using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
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
    SkillPair[] skillPairList; //¿ŒΩ∫∆Â≈Õ∑Œ Ω∫≈≥ µÒº≈≥ ∏Æ √ ±‚»≠øÎ

    private Dictionary<SkillCastType, Skill> skillCreateDic
        = new Dictionary<SkillCastType, Skill>();

    [SerializeField]
    ButtonBinder skillButtonsUI;

    private void Awake()
    {
        Instance = this;
        InitSkillCreateDic();
    }

    private void InitSkillCreateDic()
    {
        foreach (var skillPair in skillPairList)
        {
            if(skillPair.skill != null)
                skillCreateDic.Add(skillPair.skillCastType, skillPair.skill);
        }
    }

    public void InitSkillInput(SkillData[] skillDataList)
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

    public void SetSkillKey(SkillData skillData)
    {
        /*if (skillData.castType == SkillCastType.Charge)
            isChargeKey = true;
        else
            isChargeKey = false;*/

        //skillButton.onClick.AddListener(OnClick);
    }

    PlayerAttack player;
    int currentSKill = -1;
    public void TryUsingSkill(int skillIndex)
    {
        if (currentSKill != -1)
        {
            return;
        }

        if (!player.CanAttack)
        {
            return;
        }

        Skill skill = player.GetSkill(skillIndex);

        /*if(skillCoolDic.ContainsKey(skill._name))
        {
            return;
        }*/

        if (skill.castType == SkillCastType.Basic)
        {
            player.UsingSkill(skillIndex);
        }
        else if(skill.castType == SkillCastType.Charge)
        {
            currentSKill = skillIndex;
            player.StartCharge(skillIndex);
        }
    }

    public void EndSKill(int skillIndex)
    {
        if(currentSKill != skillIndex)
        {
            return;
        }

        Skill skill = player.GetSkill(currentSKill);

        if (skill.castType == SkillCastType.Charge)
        {
            player.EndCharge();
        }
        currentSKill = -1;
    }
}