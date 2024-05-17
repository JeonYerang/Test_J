using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;


public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    KeyCode[] skillKeyCodes = new KeyCode[2];
    List<Skill> skills;
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

    private void SetKeyCode()
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
        if (Input.GetKeyDown(skillKeyCodes[0]))
        {
            if (skillDic[skills[0]._name]) //스킬이 사용 가능한 상태면
                UsingSkill(0);
        }
        else if (Input.GetKeyDown(skillKeyCodes[1]))
        {
            if (skillDic[skills[1]._name])
                UsingSkill(1);
        }
        else if (Input.GetKeyDown(skillKeyCodes[2]))
        {
            if (skillDic[skills[2]._name])
                UsingSkill(2);
        }
    }

    public void UsingSkill(int num)
    {
        skills[num].UsingSkill();

        ConditionCheck(num);
    }

    public void ConditionCheck(int num)
    {
        Skill skill = skills[num];
        SkillConditionType conditionType = skill.conditionType;

        switch (conditionType)
        {
            case SkillConditionType.Basic:
                break;

            case SkillConditionType.Count:
                CheckCount(skill);
                break;

            case SkillConditionType.CoolTime:
                CoolTimeCoroutine(skill._name, skill.data.requiredAmount);
                break;

            default:
                break;
        }
    }

    private void CheckCount(Skill skill)
    {
        if (GameManager.Instance.playerAttack.attackCount > skill.data.requiredAmount)
            skillDic[skill._name] = true;
        else
            skillDic[skill._name] = false;
    }

    private IEnumerator CoolTimeCoroutine(string skillName, float sec)
    {
        skillDic[skillName] = false;
        yield return new WaitForSeconds(sec);
        skillDic[skillName] = true;
    }
}