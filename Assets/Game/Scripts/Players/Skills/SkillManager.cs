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

    public List<Skill> MySkills;
    Dictionary<string, Skill> skillKeyDic = new Dictionary<string, Skill>(); //<key string, skill>
    
    Dictionary<string, bool> skillUsableDic = new Dictionary<string, bool>(); //<skill name, is usable>
    Dictionary<string, float> skillCoolDic;



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

        foreach (var skill in MySkills)
        {
            skillUsableDic.Add(skill._name, false);
        }
    }

    public Skill currentSkill;
    private string chargingKeyFlag;
    private void Update()
    {
        ConditionCheck();

        if (Input.anyKeyDown)
        {
            string input = Input.inputString;

            if (skillKeyDic.ContainsKey(input))
            {
                currentSkill = skillKeyDic[input];

                if (currentSkill.useType == SkillUseType.Charge)
                {
                    if(chargingKeyFlag == null)
                    {
                        chargingKeyFlag = input;
                        //playerAttack.Charging();
                        print("���� ����");
                    }
                }
                else
                {
                    //playerAttack.Attack(currentSkill);
                    print("��ų ���");
                }
            }
        }
        if (chargingKeyFlag != null && Input.GetKeyUp(chargingKeyFlag))
        {
            currentSkill = skillKeyDic[chargingKeyFlag];
            chargingKeyFlag = null;

            //playerAttack.EndCharging();
            //playerAttack.Attack(currentSkill);
            print("���� ��");
        }
    }

    public void ConditionCheck()
    {
        foreach(Skill skill in MySkills)
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
            > ((ICountableSkill)skill).RequiredCount)
            skillUsableDic[skill._name] = true;
        else
            skillUsableDic[skill._name] = false;
    }

    private void CheckCoolTime(Skill skill)
    {

    }

    private IEnumerator CoolTimeCoroutine(Skill skill)
    {
        float sec = ((ICoolDownableSkill)skill).RequiredSec;

        skillUsableDic[skill._name] = false;

        while (sec > 0)
        {
            yield return new WaitForSeconds(1f);
            sec--;
        }

        skillUsableDic[skill._name] = true;
    }
}