using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    public int maxHp;
    private int currentHp = 0;
    public int CurrentHp {  get { return currentHp; } }
    public int HpAmount { get { return currentHp / maxHp; } }

    //List<SkillData> skills
    public int attackPoint;
    public float attackSpeed;

    public int attackCount { get; protected set; }

    public bool isAttacking;

    //public event EventHandler<int> GetDamageEvent;

    protected void Init()
    {
        currentHp = maxHp;
    }

    public virtual void Attack()
    {
        if (attackCount < 10) attackCount++;
    }

    public virtual void UltimateAttack()
    {
        attackCount = 0;
    }

    public void GetDamage(int damage)
    {
        currentHp -= damage;

        if(currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public void TakeHeal(int amount)
    {
        currentHp += amount;

        if (currentHp > maxHp)
            currentHp = maxHp;
    }

    private void Die()
    {
        SpawnManager.Instance.DespawnCharacter();
    }
}
