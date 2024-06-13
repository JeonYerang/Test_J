using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���ο� ��ų ������ SkillSet����
public class SkillManager : MonoBehaviour
{
    PlayerAttack playerAttack;

    public static SkillManager Instance { get; private set; }

    List<SkillSet> skills;

    Dictionary<string, Skill> skillKeyDic = new Dictionary<string, Skill>(); //<key string, skill index>
    Dictionary<Button, Skill> skillButtonDic = new Dictionary<Button, Skill>(); //<key string, skill index>

    Dictionary<string, float> skillCoolDic = new Dictionary<string, float>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        InitSkills();
        SetKeyCode();
    }

    private void InitSkills()
    {

    }

    private void SetKeyCode() //Key Manager
    {
        skillKeyDic.Clear();
    }

    public Skill currentSkill;
    private string chargingKeyFlag;

    private void Update()
    {
        if (chargingKeyFlag == null) //��¡ ������ ���� ���
        {
            if (Input.anyKeyDown)
            {
                string input = Input.inputString;

                if (skillKeyDic.ContainsKey(input))
                {
                    currentSkill = skillKeyDic[input];

                    if (currentSkill.CastType == SkillCastType.Charge)
                    {
                        if (chargingKeyFlag == null)
                        {
                            chargingKeyFlag = input;
                            playerAttack.StartCharge(currentSkill);
                            print("���� ����");
                        }
                    }
                    else
                    {
                        playerAttack.TryUsingSkill(currentSkill);
                        print("��ų ���");
                    }
                }
            }
        }
        else //��¡ ���� ���
        {
            if (Input.GetKeyUp(chargingKeyFlag))
            {
                chargingKeyFlag = null;

                playerAttack.EndCharge();
                print("���� ��");
            }
        }
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
    public IEnumerator CoolCheck()
    {
        while(true)
        {
            foreach(var skill in skillCoolDic.Keys)
            {
                skillCoolDic[skill] -= Time.deltaTime;

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