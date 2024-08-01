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
    SkillPair[] skillPairList;

    private Dictionary<SkillCastType, Skill> skillCreateDic
        = new Dictionary<SkillCastType, Skill>();

    [SerializeField]
    SkillButtonsUI skillButtonsUI;

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
        if (skillData.castType == SkillCastType.Charge)
            isChargeKey = true;
        else
            isChargeKey = false;

        //skillButton.onClick.AddListener(OnClick);
    }

    #region Press Event
    bool isChargeKey = false;
    private void OnPress()
    {
        if (isChargeKey)
        {
            longClickCheckCoroutine = StartCoroutine(LongPressCheck());
        }
    }

    float holdTime = 0.2f;
    private void OnLongPress()
    {

    }

    private void OnCancel()
    {
        if (!isChargeKey)
        {
            //shot
        }
        else
        {
            if (longClickCheckCoroutine == null)
                return; //shot
            else
                StopCoroutine(longClickCheckCoroutine);
        }
    }

    Coroutine longClickCheckCoroutine = null;
    IEnumerator LongPressCheck()
    {
        yield return new WaitForSeconds(holdTime);
        OnLongPress();
    }
    #endregion

    public void UsingSkill()
    {

    }

    #region CoolDown
    Dictionary<string, float> skillCoolDic = new Dictionary<string, float>();
    public void AddCoolDic(Skill skill)
    {
        if (skill.coolTime >= 0)
        {
            skillCoolDic.Add(skill._name, skill.coolTime);

            if (coolDownCoroutine == null)
                coolDownCoroutine = StartCoroutine(CoolDown());
        }
    }

    protected Coroutine coolDownCoroutine = null;
    private IEnumerator CoolDown()
    {
        while(true)
        {
            foreach(var skill in skillCoolDic.Keys)
            {
                skillCoolDic[skill] -= Time.deltaTime;
                //skillButtonsUI.skillButtons[]
                //    .ShowCoolTime(skillCoolDic[skill]);

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

    public bool checkIsCooling()
    {
        //skillCoolDic.ContainsKey();
        return false;
    }
    #endregion
}