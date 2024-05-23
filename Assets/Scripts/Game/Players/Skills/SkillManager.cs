using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    PlayerAttack playerAttack;

    KeyCode attackKeyCode;
    KeyCode skillKeyCode;
    KeyCode ultimateKeyCode;

    public List<Skill> skills;
    Dictionary<string, Skill> skillKeyDic = new Dictionary<string, Skill>(); //<key string, skill>
    Dictionary<string, bool> skillUsableDic = new Dictionary<string, bool>(); //<skill name, is usable>

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
        skillKeyDic.Clear();

        attackKeyCode = KeyCode.Z;
        skillKeyCode = KeyCode.X;
        ultimateKeyCode = KeyCode.C;
    }

    private void InitSkillDic()
    {
        skillUsableDic.Clear();

        foreach (var skill in skills)
        {
            skillUsableDic.Add(skill._name, false);
        }
    }

    public Skill currentSkill;
    private string chargingKeyFlag;
    private void Update()
    {
        ConditionCheck();

        if (UnityEngine.Input.anyKeyDown)
        {
            string input = UnityEngine.Input.inputString;

            if (skillKeyDic.ContainsKey(input))
            {
                currentSkill = skillKeyDic[input];

                if (currentSkill.useType == UseType.Charge)
                {
                    if(chargingKeyFlag == null)
                    {
                        chargingKeyFlag = input;
                        //playerAttack.Charging();
                        print("차지 시작");
                    }
                }
                else
                {
                    //playerAttack.Attack(currentSkill);
                    print("스킬 사용");
                }
            }
        }
        if (chargingKeyFlag != null && UnityEngine.Input.GetKeyUp(chargingKeyFlag))
        {
            currentSkill = skillKeyDic[chargingKeyFlag];
            chargingKeyFlag = null;

            //playerAttack.EndCharging();
            //playerAttack.Attack(currentSkill);
            print("차지 끝");
        }
    }

    public void ConditionCheck()
    {
        foreach(Skill skill in skills)
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
                    CheckCoolTime(skill);
                    break;

                default:
                    break;
            }
        }
    }

    private void CheckCount(Skill skill)
    {
        if (GameManager.Instance.playerAttack.AttackCount 
            > ((ICountActivatableSkill)skill).RequiredCount)
            skillUsableDic[skill._name] = true;
        else
            skillUsableDic[skill._name] = false;
    }

    private void CheckCoolTime(Skill skill)
    {

    }

    private IEnumerator CoolTimeCoroutine(Skill skill)
    {
        float sec = ((ICoolTimeActivatableSkill)skill).RequiredSec;

        skillUsableDic[skill._name] = false;

        while (sec > 0)
        {
            yield return new WaitForSeconds(1f);
            sec--;
        }

        skillUsableDic[skill._name] = true;
    }
}