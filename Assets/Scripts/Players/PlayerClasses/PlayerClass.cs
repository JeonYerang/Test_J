using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerClass : MonoBehaviour
{
    public int maxHp;
    private int currentHp = 0;

    public int attackPoint;
    public float attackSpeed;
    public float attackCoolTime;

    private void Awake()
    {
        currentHp = maxHp;
    }

    public abstract void Attack();
    public void GetDamage(int damage)
    {
        currentHp -= damage;
    }

    public void TakeHeal(int amount)
    {
        currentHp += amount;
    }
}
