using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    public enum AttackState
    {
        Idle,
        Attack,
        Charge,
        BeAttacked,
        Die
    }
    public AttackState state;

    public int maxHp;
    protected int currentHp = 0;
    public int CurrentHp { get { return currentHp; } }
    public int HpAmount { get { return currentHp / maxHp; } }

    public int attackPoint;
    public float attackSpeed;

    public bool CanAttack { get { return state == AttackState.Idle; } }

    PlayerClass playerClass;
    
    public event EventHandler<Player> onChangedHp;

    public Animator animator;

    protected void Init()
    {
        currentHp = maxHp;
        state = AttackState.Idle;

        onChangedHp += GameUIManager.Instance.UserInfo.SetHpBar;
    }

    public void SetClass()
    {

    }

    public void SetAnimator(string name)
    {

    }

    #region Charge
    Skill chargingSkill;
    public virtual void StartCharge(Skill skill)
    {
        chargingSkill = skill;
        state = AttackState.Charge;

        //animator.SetTrigger("");
    }

    public virtual void EndCharge()
    {
        //animator.SetTrigger("");
    }
    #endregion

    #region Attack
    public int AttackCount { get; protected set; }
    
    public void TryUsingSkill(Skill skill)
    {
        if (!CanAttack)
            return;

        UsingSkill(skill);
    }

    private void UsingSkill(Skill skill) //스킬 인덱스로 참조하는 건?
    {
        state = AttackState.Attack;

        //animator.SetTrigger(animationName);
        skill.Shot();

        AttackCount++;
    }

    protected void ReturnIdleState()
    {
        state = AttackState.Idle;
    }
    #endregion

    #region About Hp
    public void GetDamage(int damage)
    {
        if (state == AttackState.Die)
            return;

        //state = AttackState.BeAttacked;
        currentHp -= damage;

        if(currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }

        Player player = GetComponent<PlayerInfo>().player;
        onChangedHp?.Invoke(this, player);
    }

    public void TakeHeal(int amount)
    {
        if (state == AttackState.Die)
            return;

        currentHp += amount;

        if (currentHp > maxHp)
            currentHp = maxHp;

        Player player = GetComponent<PlayerInfo>().player;
        onChangedHp?.Invoke(this, player);
    }

    private void Die()
    {
        state = AttackState.Die;
        SpawnManager.Instance.DespawnCharacter();
    }
    #endregion
}
