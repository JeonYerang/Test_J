using System;
using UnityEngine;

public enum SkillCastType
{
    Basic,
    Charge,
    Combo,
    OnOff
}

[Serializable]
public abstract class Skill : MonoBehaviour
{
    protected PlayerAttack owner;

    protected SkillData skillData;

    public string _name => skillData._name;
    public int damage => skillData.damage;
    public float coolTime => skillData.coolTime;
    public SkillCastType castType => skillData.castType;

    public virtual void Init(SkillData skillData)
    {
        this.skillData = skillData;
    }

    public void SetOwner(PlayerAttack owner)
    {
        this.owner = owner;
    }

    public abstract void Shot();
}