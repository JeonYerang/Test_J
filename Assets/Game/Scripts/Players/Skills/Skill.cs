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

#region <스킬 타입>
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

    

    public override void Shot() //콤보공격
    {
        //스킬
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


//Q.왜 추상클래스가 아닌 인터페이스?
//A.스킬의 조건과 스킬의 사용 방법을 각각 상속받기 위해
//예를 들면 쿨타임이 있으면서, 차징이 가능한 스킬
//추상클래스는 하나밖에 상속을 못 받으니까...
//기능을 확장한다기보다는 어떤 스킬이 가지고 있는 기능 중 하나로 정의하고 싶었음.