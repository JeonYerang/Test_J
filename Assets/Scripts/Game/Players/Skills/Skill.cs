using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    PlayerAttack owner;

    public string _name;
    public SkillData data;

    public SkillConditionType conditionType;
    public UseType useType;

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
public enum UseType
{
    Basic,
    OnOff,
    Charge
}

public interface IOnOffableSkill
{
    bool IsOn { get; set; }
    void UnUsingSkill();
}

public interface IChargableSkill
{
    int MaxChargeCount { get; set; }
    int CurrentChargeCount { get; set; }
    void Charging();
    void EndCharging();
}
#endregion

public class ArrowShot : Skill, IChargableSkill
{
    public int MaxChargeCount { get; set; }
    public int CurrentChargeCount { get; set; }

    public void Charging()
    {
        StartCoroutine(ChargeCoroutine());
    }

    public void EndCharging()
    {
        
    }

    IEnumerator ChargeCoroutine()
    {
        yield return new WaitForSeconds(3);
    }

    public override void UsingSkill()
    {
        throw new System.NotImplementedException();
    }
}