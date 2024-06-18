using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//새로운 스킬 제작은 SkillSet으로
public class SkillManager : MonoBehaviour
{
    PlayerAttack playerAttack;

    public static SkillManager Instance { get; private set; }

    Dictionary<string, float> skillCoolDic = new Dictionary<string, float>();

    KeyBinder keyManager;
    [SerializeField]
    SkillButtonsUI skillButtonsUI;

    private void Awake()
    {
        Instance = this;
    }

    public int currentSkillIndex;
    private string chargingKeyFlag;

    private Dictionary<SkillCastType, Type> skillClassDic
        = new Dictionary<SkillCastType, Type>()
        {
            { SkillCastType.Basic, typeof(BasicSkill) },
            { SkillCastType.Charge, typeof(ChargeSkill) },
            { SkillCastType.Combo, typeof(ComboSkill) },
            { SkillCastType.OnOff, typeof(OnOffSkill) },
        };

    private void InitSkills(SkillSet[] skillSets)
    {
        skillButtonsUI.InitSkillButtons(skillSets);
    }

    public Skill[] GetSkillList(SkillSet[] skillSets)
    {
        Skill[] skillList = new Skill[skillSets.Length];

        for (int i = 0; i < skillSets.Length; i++)
        {
            Type classType = skillClassDic[skillSets[i].castType];
            Skill createdSkill = (Skill)Activator.CreateInstance(classType);
            skillList[i] = createdSkill;
        }

        return skillList;
    }

    public void TryUsingSkill(int skillIndex)
    {
        if (GameManager.Instance.playerAttack != null)
            GameManager.Instance.playerAttack.TryUsingSkill(skillIndex);
    }

    public void TryChargingSkill(int skillIndex)
    {
        //if (GameManager.Instance.playerAttack != null)
        //    GameManager.Instance.playerAttack.TryChargingSkill(skillIndex);
    }

    #region CoolDown
    public void AddCoolDic(Skill skill)
    {
        if (skill.CoolTime >= 0)
        {
            skillCoolDic.Add(skill.name, skill.CoolTime);

            if (conditionCheckCoroutine == null)
                conditionCheckCoroutine = StartCoroutine(CoolCheck());
        }
    }

    protected Coroutine conditionCheckCoroutine = null;
    private IEnumerator CoolCheck()
    {
        while(true)
        {
            foreach(var skill in skillCoolDic.Keys)
            {
                skillCoolDic[skill] -= Time.deltaTime;
                skillButtonsUI.skillButtons[currentSkillIndex]
                    .ShowCoolTime(skillCoolDic[skill]);

                if (skillCoolDic[skill] <= 0)
                {
                    skillCoolDic.Remove(skill);

                    if(skillCoolDic.Count == 0)
                        yield break;
                }
            }
            yield return null;
        }
    }
    #endregion
}