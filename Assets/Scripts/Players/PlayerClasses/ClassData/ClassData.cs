using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassData : ScriptableObject
{
    public int maxHp;

    public int attackPoint;
    public float attackSpeed;
    public float attackCoolTime;

    public Projectile attackPrefab;
    public PlayerAttack playerAttack;
}
