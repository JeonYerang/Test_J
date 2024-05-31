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
    List<Skill> skills = new List<Skill>();

    public event EventHandler<Player> onChangedHp;

    public Animator animator;

    protected void Init()
    {
        currentHp = maxHp;
        state = AttackState.Idle;

        onChangedHp += GameUIManager.Instance.UserInfo.SetHpBar;
    }

    #region Charge
    public virtual void StartCharge()
    {
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
    
    public virtual void UsingSkill(Skill skill) //스킬 인덱스로 참조하는 건?
    {
        state = AttackState.Attack;

        //animator.SetTrigger(animationName);
        skill.UsingSkill();

        if (AttackCount < 10) AttackCount++;
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
