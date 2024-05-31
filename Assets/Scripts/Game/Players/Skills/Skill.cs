using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerAttack;

public abstract class Skill : MonoBehaviour
{
    protected PlayerAttack owner;

    public string _name;
    public SkillData data;

    public SkillConditionType conditionType;
    public SkillUseType useType;

    public void Init(SkillData data)
    {
        this.data = data;
    }

    public void SetOwner()
    {
        this.owner = GameManager.Instance.playerAttack;
    }

    public abstract void UsingSkill();
}

#region <사용 조건>
public enum SkillConditionType
{
    Basic,
    CoolTime,
    Count
}

public interface ICoolDownableSkill
{
    float RequiredSec { get; set; }
    float RemainSec { get; set; }
}

public interface ICountableSkill
{
    int RequiredCount { get; set; }
}
#endregion

#region <사용 방법>
public enum SkillUseType
{
    Basic,
    Charge,
    Combo,
    OnOff
}

public class ChargeSkill : Skill
{
    public float maxChargeCount;
    public float CurrentChargeCount { get; set; }

    public virtual void StartCharge()
    {
        owner.StartCharge();
        chargeCoroutine = StartCoroutine(ChargeCoroutine());
    }

    public virtual void EndCharge()
    {
        if (chargeCoroutine != null)
            StopCoroutine(ChargeCoroutine());

        UsingSkill();
    }

    protected Coroutine chargeCoroutine = null;
    protected IEnumerator ChargeCoroutine()
    {
        CurrentChargeCount = 0;
        while (CurrentChargeCount < maxChargeCount)
        {
            CurrentChargeCount += Time.deltaTime;
            yield return null;
        }
    }

    public override void UsingSkill()
    {
        //차징카운트에 따라...
    }
}
public class ComboSkill : Skill
{
    public GameObject[] comboAttackPrefabs;
    public string[] comboAnimationNames;
    public int[] comboDamages;
    public int maxComboCount;
    public int currentComboCount { get; set; }

    public override void UsingSkill() //콤보공격
    {
        Instantiate(comboAttackPrefabs[currentComboCount], 
            transform.position, transform.rotation);

        if (comboCoroutine != null) StopCoroutine(comboCoroutine);

        if (currentComboCount < maxComboCount)
        {
            currentComboCount++;
            comboCoroutine = StartCoroutine(ComboCoroutine());
        }
        else
            currentComboCount = 0;
    }

    Coroutine comboCoroutine = null;
    private IEnumerator ComboCoroutine()
    {
        yield return new WaitForSeconds(3);
        currentComboCount = 0;
    }
}
public class OnOffSkill : Skill
{
    GameObject skillObj;
    public bool IsOn { get; set; }

    public void On()
    {
        if(skillObj == null)
        {
            skillObj = Instantiate(data.skillPrefab,
            owner.transform.position, owner.transform.rotation, owner.transform);
        }
        else
            skillObj.gameObject.SetActive(true);
    }

    public void Off()
    {
        skillObj.gameObject.SetActive(false);
    }

    public override void UsingSkill()
    {
        if(IsOn) Off();
        else On();
    }
}
#endregion