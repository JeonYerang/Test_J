using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public int maxHp;
    private int currentHp = 0;

    //List<Skill> skills
    public int attackPoint;
    public float attackSpeed;
    public float attackCoolTime;

    public Projectile attackPrefab;

    private void Awake()
    {

    }

    protected void Init()
    {
        currentHp = maxHp;
    }

    public virtual void Attack()
    {
        Projectile projectile = Instantiate(attackPrefab);
        projectile.Shot(GetComponent<PlayerInfo>());
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
