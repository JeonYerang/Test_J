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

    public string Name {  get; protected set; }

    public int Damage {  get; protected set; }
    public float CoolTime {  get; protected set; }

    public SkillCastType CastType {  get; protected set; }

    public virtual void Init(SkillSet set)
    {
        Name = set.name;
        Damage = set.damage;
        CoolTime = set.coolTime;
        CastType = set.castType;
    }

    public void SetOwner(PlayerAttack owner)
    {
        this.owner = owner;
    }

    public abstract void Shot();
}

//Q.왜 추상클래스가 아닌 인터페이스?
//A.스킬의 조건과 스킬의 사용 방법을 각각 상속받기 위해
//예를 들면 쿨타임이 있으면서, 차징이 가능한 스킬
//추상클래스는 하나밖에 상속을 못 받으니까...
//기능을 확장한다기보다는 어떤 스킬이 가지고 있는 기능 중 하나로 정의하고 싶었음.