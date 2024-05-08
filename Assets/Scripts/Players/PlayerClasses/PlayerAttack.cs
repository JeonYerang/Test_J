using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int maxHp;
    private int currentHp = 0;

    public int attackPoint;
    public float attackSpeed;
    public float attackCoolTime;

    public Projectile attackPrefab;

    private void Awake()
    {

    }

    protected void Init(ClassData classData)
    {
        //this.maxHp = classData.maxHp;
        //this.attackPoint = classData.attackPoint;
        //this.attackSpeed = classData.attackSpeed;
        //this.attackCoolTime = classData.attackCoolTime;

        currentHp = maxHp;
    }

    public virtual void Attack()
    {
        Projectile projectile = Instantiate(attackPrefab);
        projectile.InitAndShot(GetComponent<PlayerInfo>(), attackPoint, attackSpeed);
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

    }
}
