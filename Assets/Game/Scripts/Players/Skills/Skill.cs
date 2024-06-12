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

    public override void Shot() //콤보공격
    {
        //스킬
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

//Q.왜 추상클래스가 아닌 인터페이스?
//A.스킬의 조건과 스킬의 사용 방법을 각각 상속받기 위해
//예를 들면 쿨타임이 있으면서, 차징이 가능한 스킬
//추상클래스는 하나밖에 상속을 못 받으니까...
//기능을 확장한다기보다는 어떤 스킬이 가지고 있는 기능 중 하나로 정의하고 싶었음.