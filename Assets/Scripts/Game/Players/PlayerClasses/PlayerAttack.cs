using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttack : MonoBehaviour
{
    public int maxHp;
    private int currentHp = 0;

    //List<Skill> skills
    public int attackPoint;
    public float attackSpeed;
    public float attackCoolTime;

    public bool isAttacking;

    //public event EventHandler<int> GetDamageEvent;

    private void Update()
    {
        
    }

    protected void Init()
    {
        currentHp = maxHp;
    }

    public abstract void Attack();

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
        SpawnManager.instance.DespawnCharacter();
    }
}
