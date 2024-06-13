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

//Q.�� �߻�Ŭ������ �ƴ� �������̽�?
//A.��ų�� ���ǰ� ��ų�� ��� ����� ���� ��ӹޱ� ����
//���� ��� ��Ÿ���� �����鼭, ��¡�� ������ ��ų
//�߻�Ŭ������ �ϳ��ۿ� ����� �� �����ϱ�...
//����� Ȯ���Ѵٱ⺸�ٴ� � ��ų�� ������ �ִ� ��� �� �ϳ��� �����ϰ� �;���.