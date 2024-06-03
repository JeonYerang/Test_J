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

    public override void UsingSkill() //콤보공격
    {
        //스킬

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

//Q.왜 추상클래스가 아닌 인터페이스?
//A.스킬의 조건과 스킬의 사용 방법을 각각 상속받기 위해
//예를 들면 쿨타임이 있으면서, 차징이 가능한 스킬
//추상클래스는 하나밖에 상속을 못 받으니까...
//기능을 확장한다기보다는 어떤 스킬이 가지고 있는 기능 중 하나로 정의하고 싶었음.