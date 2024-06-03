using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected PlayerAttack owner;

    public string _name;
    public SkillData data;

    public float damage;

    public SkillConditionType conditionType;
    public SkillUseType useType;

    public void Init(SkillData data)
    {
        this.data = data;
        damage = data.damage;
    }

    public void SetOwner()
    {
        this.owner = GameManager.Instance.playerAttack;
    }

    public abstract void UsingSkill();
}

#region <��� ����>
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

#region <��� ���>
public enum SkillUseType
{
    Basic,
    Charge,
    Combo,
    OnOff
}

public abstract class ChargeSkill : Skill
{
    public float maxChargeCount;
    public float CurrentChargeCount { get; set; }

    public GameObject ChargingEffect;

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
}
public abstract class ComboSkill : Skill
{
    public int maxComboCount;
    public int currentComboCount { get; set; }

    public override void UsingSkill() //�޺�����
    {
        //��ų

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
public abstract class OnOffSkill : Skill
{
    public bool IsOn { get; set; }

    protected abstract void On();

    protected abstract void Off();

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