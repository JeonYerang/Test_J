using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.UI.GridLayoutGroup;

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

    public float Damage {  get; protected set; }
    public float CoolTime {  get; protected set; }

    public SkillCastType CastType {  get; protected set; }

    public virtual void Init(SkillData data)
    {
        Name = data.name;
        Damage = data.damage;
        CoolTime = data.coolTime;
        CastType = data.castType;
    }

    public void SetOwner(PlayerAttack owner)
    {
        this.owner = owner;
    }

    public abstract void Shot();
}

public class ChargeSkill : Skill
{
    public float maxChargeCount;
    public float CurrentChargeCount { get; private set; }

    public string skillAnimation;
    public SkillObject skillPrefab;

    public GameObject ChargingEffect;
    
    public bool IsCharging { get; private set; }


    public void StartCharge()
    {
        chargeCoroutine = StartCoroutine(ChargeCoroutine());
        IsCharging = true;
    }

    public void EndCharge()
    {
        if (chargeCoroutine != null)
            StopCoroutine(ChargeCoroutine());
        IsCharging = false;

        Shot();
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

    public override void Shot()
    {
        Instantiate(skillPrefab, owner.transform.position, owner.transform.rotation);
        owner.SetAnimator(skillAnimation);
    }
}

public class ComboSkill : Skill
{
    public int maxComboCount;
    public int currentComboCount { get; set; }

    public string[] skillAnimation;
    public SkillObject[] skillPrefab;

    public override void Init(SkillData data)
    {
        base.Init(data);

        /*if (data is ComboSkillData)
        {
            //ComboSkillData comboSkillData = data as ComboSkillData;   
            //this.maxComboCount = comboSkillData.maxComboCount;
        }*/
    }

    public override void Shot() //�޺�����
    {
        //��ų
        owner.SetAnimator(skillAnimation[currentComboCount]);
        Instantiate(skillPrefab[currentComboCount], owner.transform.position, owner.transform.rotation);

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
    public string skillAnimation;
    public SkillObject skillPrefab;

    public SkillObject skillObject;
    public bool IsOn { get; set; }

    protected void On()
    {
        if (skillObject == null)
        {
            skillObject = Instantiate(skillPrefab,
                owner.transform.position, owner.transform.rotation, owner.transform);
        }
        else
            skillObject.gameObject.SetActive(true);
        
    }

    protected void Off()
    {
        if (skillObject == null)
            return;

        skillObject.gameObject.SetActive(false);
    }

    public override void Shot()
    {
        if(IsOn) Off();
        else On();
    }
}

//Q.�� �߻�Ŭ������ �ƴ� �������̽�?
//A.��ų�� ���ǰ� ��ų�� ��� ����� ���� ��ӹޱ� ����
//���� ��� ��Ÿ���� �����鼭, ��¡�� ������ ��ų
//�߻�Ŭ������ �ϳ��ۿ� ����� �� �����ϱ�...
//����� Ȯ���Ѵٱ⺸�ٴ� � ��ų�� ������ �ִ� ��� �� �ϳ��� �����ϰ� �;���.