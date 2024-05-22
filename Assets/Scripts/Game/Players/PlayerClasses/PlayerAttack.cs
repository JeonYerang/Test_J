using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
    private int currentHp = 0;
    public int CurrentHp { get { return currentHp; } }
    public int HpAmount { get { return currentHp / maxHp; } }

    //List<SkillData> skills
    public int attackPoint;
    public float attackSpeed;

    public bool CanAttack { get { return state == AttackState.Idle; } }

    //public event EventHandler<int> GetDamageEvent;

    protected void Init()
    {
        currentHp = maxHp;
        state = AttackState.Idle;
    }

    #region Attack
    public int AttackCount { get; protected set; }
    public virtual void Attack()
    {
        state = AttackState.Attack;

        if (AttackCount < 10) AttackCount++;
    }

    public virtual void UsingSkill(Skill skill)
    {
        state = AttackState.Attack;

        skill.UsingSkill();

        if (AttackCount < 10) AttackCount++;
    }

    public virtual void UsingUltimateSkill()
    {
        state = AttackState.Attack;

        AttackCount = 0;
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

        state = AttackState.BeAttacked;
        currentHp -= damage;

        if(currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public void TakeHeal(int amount)
    {
        if (state == AttackState.Die)
            return;

        currentHp += amount;

        if (currentHp > maxHp)
            currentHp = maxHp;
    }

    private void Die()
    {
        state = AttackState.Die;
        StartCoroutine(DespawnCharacterCoroutine());
    }

    private IEnumerator DespawnCharacterCoroutine()
    {
        yield return new WaitForSeconds(3f);
        SpawnManager.Instance.DespawnCharacter();
    }
    #endregion
}
