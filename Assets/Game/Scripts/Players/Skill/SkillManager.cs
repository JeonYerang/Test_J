using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//���ο� ��ų ������ SkillSet����
public class SkillManager : MonoBehaviour
{
    PlayerAttack playerAttack;

    public static SkillManager Instance { get; private set; }

    Skill[] skills;

    Dictionary<string, float> skillCoolDic = new Dictionary<string, float>();

    KeyManager keyManager;
    [SerializeField]
    SkillButtonsUI skillButtonsUI;

    private void Awake()
    {
        Instance = this;
    }

    public int currentSkillIndex;
    private string chargingKeyFlag;
    /*private void Update()
    {
        if (chargingKeyFlag == null) //��¡ ������ ���� ���
        {
            
            if (Input.anyKeyDown)
            {
                string input = Input.inputString;

                if (KeySetting.skillKeyDic.ContainsKey(input))
                {
                    currentSkillIndex = KeySetting.skillKeyDic[input];

                    if (currentSkillIndex.CastType == SkillCastType.Charge)
                    {
                        if (chargingKeyFlag == null)
                        {
                            chargingKeyFlag = input;
                            //playerAttack.StartCharge(currentSkill);
                            print("���� ����");
                        }
                    }
                    else
                    {
                        TryUsingSkill(currentSkillIndex);
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

                //playerAttack.EndCharge();
                print("���� ��");
            }
        }

        if (Input.GetKeyDown(KeySetting.jumpKey))
        {
            if (GameManager.Instance.playerMove != null)
                GameManager.Instance.playerMove.OnJump();
        }
    }*/

    private void InitSkills(SkillSet[] skillSets)
    {
        skills = new Skill[skillSets.Length];

        for(int i = 0; i < skillSets.Length; i++)
        {
            skills[i] = null;
        }

        skillButtonsUI.InitSkillButtons(skillSets);
    }

    public void TryUsingSkill(int skillIndex)
    {
        bool isUsed = false;
        Skill targetSkill = skills[skillIndex];

        if (GameManager.Instance.playerAttack != null)
        {
            isUsed
                = GameManager.Instance.playerAttack.TryUsingSkill(targetSkill);
        }

        if(isUsed)
            AddCoolDic(targetSkill);
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