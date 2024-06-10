using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

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

                if (currentSkill.type == SkillType.Charge)
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
        if (chargingKeyFlag != null && Input.GetKeyUp(chargingKeyFlag))
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
        foreach(Skill skill in MySkills)
        {
            SkillType type = skill.type;

            /*switch (type)
            {
                

                default:
                    break;
            }*/
        }
    }

    /*public void CoolTimeCheck()
    {
        if (coolTimeCoroutine != null)
            coolTimeCoroutine = StartCoroutine(CoolTimeCoroutine());
    }*/

    protected Coroutine coolTimeCoroutine = null;
    /*protected IEnumerator CoolTimeCoroutine()
    {
        RemainSec = requiredSec;

        canUse = false;
        while (RemainSec > 0)
        {
            yield return new WaitForSeconds(1f);
            RemainSec--;
        }
        canUse = true;
    }*/
}