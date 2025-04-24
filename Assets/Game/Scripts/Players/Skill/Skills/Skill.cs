using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
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

    public event Action<float> OnCooldownChanged;
    //skill.OnCooldownChanged += UpdateCooldownBar; 구독자 클래스에 추가

    public virtual void Init(SkillData skillData)
    {
        this.skillData = skillData;
    }

    public void SetOwner(PlayerAttack owner)
    {
        this.owner = owner;
    }

    public virtual void Shot()
    {
        if(coolTime > 0)
        {
            StartCoolDown();
        }
    }

    [PunRPC]
    public abstract void InstantiateEffect(Vector3 shotPos, Quaternion shotDir, int damage, Player target);


    #region CoolDown
    private float currentCoolTime = 0f;
    public float CurrentCoolTime
    {
        get
        {
            return currentCoolTime;
        }
        set
        {
            currentCoolTime = value;
            OnCooldownChanged?.Invoke(value);
        }
    }

    private void StartCoolDown()
    {
        CurrentCoolTime = coolTime;

        if (coolDownCoroutine == null)
            coolDownCoroutine = StartCoroutine(CoolDown());
    }

    protected Coroutine coolDownCoroutine = null;
    private IEnumerator CoolDown()
    {
        while(CurrentCoolTime > 0)
        {
            CurrentCoolTime -= Time.deltaTime;
        }
        yield break;
    }
    #endregion

}