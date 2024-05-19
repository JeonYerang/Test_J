using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    KeyCode[] skillKeyCodes = new KeyCode[2];
    public List<Skill> skills;
    Dictionary<string, bool> skillDic = new Dictionary<string, bool>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetKeyCode();
        InitSkillDic();
    }

    private void SetKeyCode() //Key Manager
    {
        skillKeyCodes[0] = KeyCode.Z;
        skillKeyCodes[1] = KeyCode.X;
        skillKeyCodes[2] = KeyCode.C;
    }

    private void InitSkillDic()
    {
        skillDic.Clear();

        foreach(var skill in skills)
        {
            skillDic.Add(skill._name, false);
        }
    }

    private void Update()
    {
        for (int i = 0; i < skillKeyCodes.Length; i++)
        {
            Skill skill = skills[i];
            ConditionCheck(skill);

            switch (skill.useType)
            {
                case UseType.Basic:
                    if (Input.GetKeyDown(skillKeyCodes[i]))
                    {
                        if (skillDic[skill._name]) //스킬이 사용 가능한 상태면
                            skill.UsingSkill();
                    }
                    break;

                case UseType.OnOff:
                    if (Input.GetKeyDown(skillKeyCodes[i]))
                    {
                        if (skillDic[skill._name]) //스킬이 사용 가능한 상태면
                        {
                            if (!((IOnOffableSkill)skill).IsOn)
                                skill.UsingSkill();
                            else
                                ((IOnOffableSkill)skill).UnUsingSkill();
                        }
                    }
                    break;

                case UseType.Charge:
                    if (Input.GetKey(skillKeyCodes[i]))
                    {
                        ((IChargableSkill)skill).Charging();
                    }
                    if (Input.GetKeyUp(skillKeyCodes[i]))
                    {
                        skill.UsingSkill();
                    }
                    break;
            }
        }
    }

    public void ConditionCheck(Skill skill)
    {
        SkillConditionType conditionType = skill.conditionType;

        switch (conditionType)
        {
            case SkillConditionType.Basic:
                break;

            case SkillConditionType.Count:
                CheckCount(skill);
                break;

            case SkillConditionType.CoolTime:
                CoolTimeCoroutine(skill);
                break;

            default:
                break;
        }
    }

    private void CheckCount(Skill skill)
    {
        if (GameManager.Instance.playerAttack.attackCount 
            > ((ICountActivatableSkill)skill).RequiredCount)
            skillDic[skill._name] = true;
        else
            skillDic[skill._name] = false;
    }

    private IEnumerator CoolTimeCoroutine(Skill skill)
    {
        float sec = ((ICoolTimeActivatableSkill)skill).RequiredSec;

        skillDic[skill._name] = false;

        while (sec > 0)
        {
            yield return new WaitForSeconds(1f);
            sec--;
        }

        skillDic[skill._name] = true;
    }
}