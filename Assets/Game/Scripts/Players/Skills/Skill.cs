using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class Skill : MonoBehaviour
{
    protected PlayerAttack owner;

    public SkillData data;

    public string _name;

    public float coolTime;
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

    public virtual void Shot()
    {
        Instantiate(data.skillPrefab[0], owner.transform.position, owner.transform.rotation);
        owner.SetAnimator(data.skillAnimation[0]);
    }
}

#region <��ų Ÿ��>
public enum SkillType
{
    Basic,
    Charge,
    Combo,
    OnOff
}

public class SkillRealeser : MonoBehaviour
{
    public string[] skillAnimation;
    public GameObject[] skillPrefab;

    public virtual void Shot()
    {
        //Instantiate(data.skillPrefab[0], owner.transform.position, owner.transform.rotation);
        //owner.SetAnimator(data.skillAnimation[0]);
    }
}

public class ChargeSkill : Skill
{
    public float maxChargeCount;
    public float CurrentChargeCount { get; private set; }

    public GameObject ChargingEffect;
    
    public bool IsCharging { get; private set; }
    public Skill ChargingSkill;
    public void StartCharge(Skill skill)
    {
        chargeCoroutine = StartCoroutine(ChargeCoroutine());
        IsCharging = true;
    }

    public void EndCharge()
    {
        if (chargeCoroutine != null)
            StopCoroutine(ChargeCoroutine());
        IsCharging = false;

        //Shot();
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
public class ComboSkill : Skill
{
    public int maxComboCount;
    public int currentComboCount { get; set; }

    

    public override void Shot() //�޺�����
    {
        //��ų
        owner.SetAnimator(data.skillAnimation[currentComboCount]);
        Instantiate(data.skillPrefab[currentComboCount], owner.transform.position, owner.transform.rotation);

        ComboSet();
    }

    protected void ComboSet()
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
    public GameObject skillObject;
    public bool IsOn { get; set; }

    protected void On()
    {
        if (skillObject == null)
        {
            skillObject = Instantiate(data.skillPrefab[0],
                owner.transform.position, owner.transform.rotation, owner.transform);
        }
        else
            skillObject.SetActive(true);
        
    }

    protected void Off()
    {
        if (skillObject == null)
            return;

        skillObject.SetActive(false);
    }

    public override void Shot()
    {
        if(IsOn) Off();
        else On();
    }
}
#endregion

public class SkillEffects
{
    string animationName;
    ParticleSystem particle;
    GameObject skillPrefab;
}


//Q.�� �߻�Ŭ������ �ƴ� �������̽�?
//A.��ų�� ���ǰ� ��ų�� ��� ����� ���� ��ӹޱ� ����
//���� ��� ��Ÿ���� �����鼭, ��¡�� ������ ��ų
//�߻�Ŭ������ �ϳ��ۿ� ����� �� �����ϱ�...
//����� Ȯ���Ѵٱ⺸�ٴ� � ��ų�� ������ �ִ� ��� �� �ϳ��� �����ϰ� �;���.