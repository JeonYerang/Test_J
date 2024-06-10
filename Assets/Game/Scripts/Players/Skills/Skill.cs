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

#region <��ų Ÿ��>
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

    public override void UsingSkill() //�޺�����
    {
        //��ų
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




//Q.�� �߻�Ŭ������ �ƴ� �������̽�?
//A.��ų�� ���ǰ� ��ų�� ��� ����� ���� ��ӹޱ� ����
//���� ��� ��Ÿ���� �����鼭, ��¡�� ������ ��ų
//�߻�Ŭ������ �ϳ��ۿ� ����� �� �����ϱ�...
//����� Ȯ���Ѵٱ⺸�ٴ� � ��ų�� ������ �ִ� ��� �� �ϳ��� �����ϰ� �;���.