using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class Skill : MonoBehaviour
{
    protected PlayerAttack owner;
    public bool canUse;

    public string _name;
    public SkillData data;

    public float damage;

    public SkillType type;

    public void Init(SkillData data)
    {
        this.data = data;
        damage = data.damage;
    }

    public void SetOwner(PlayerAttack owner)
    {
        this.owner = owner;
    }

    public abstract void UsingSkill();
}

public enum SkillConditionType
{
    None,
    CoolTime,
    Count
}

#region <스킬 타입>
public enum SkillType
{
    Basic,
    Charge,
    Combo,
    OnOff
}

public class SkillUseType
{

}

public class SkillEffects
{
    string animationName;
    ParticleSystem particle;
    GameObject skillPrefab;
}

public class CoolTimeSkill : Skill
{
    public float requiredSec;
    public float RemainSec { get; private set; }

    public void StartCoolTime()
    {
        if (coolTimeCoroutine != null)
            coolTimeCoroutine = StartCoroutine(CoolTimeCoroutine());
    }

    protected Coroutine coolTimeCoroutine = null;
    protected IEnumerator CoolTimeCoroutine()
    {
        RemainSec = requiredSec;

        canUse = false;
        while (RemainSec > 0)
        {
            yield return new WaitForSeconds(1f);
            RemainSec--;
        }
        canUse = true;
    }

    public override void UsingSkill()
    {
    }
}

public abstract class CountSkill : Skill
{
    public int RequiredCount { get; private set; }

    public void CheckCount()
    {
        if (RequiredCount > owner.AttackCount)
            canUse = true;
        else
            canUse = false;
    }
}

public class ChargeSkill : Skill
{
    public float maxChargeCount;
    public float CurrentChargeCount { get; private set; }

    public GameObject ChargingEffect;
    public bool IsCharging {  get; private set; }

    public void StartCharge()
    {
        owner.StartCharge();
        chargeCoroutine = StartCoroutine(ChargeCoroutine());
        IsCharging = true;
    }

    public void EndCharge()
    {
        if (chargeCoroutine != null)
            StopCoroutine(ChargeCoroutine());
        IsCharging = false;

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
        throw new System.NotImplementedException();
    }
}
public class ComboSkill : Skill
{
    public int maxComboCount;
    public int currentComboCount { get; set; }

    public override void UsingSkill() //콤보공격
    {
        //스킬
        //animator.SetTrigger("Attack");
        ComboCheck();
    }

    protected void ComboCheck()
    {
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
    public bool IsOn { get; set; }

    protected void On() { }

    protected void Off() { }

    public override void UsingSkill()
    {
        if(IsOn) Off();
        else On();
    }
}
#endregion




//Q.왜 추상클래스가 아닌 인터페이스?
//A.스킬의 조건과 스킬의 사용 방법을 각각 상속받기 위해
//예를 들면 쿨타임이 있으면서, 차징이 가능한 스킬
//추상클래스는 하나밖에 상속을 못 받으니까...
//기능을 확장한다기보다는 어떤 스킬이 가지고 있는 기능 중 하나로 정의하고 싶었음.