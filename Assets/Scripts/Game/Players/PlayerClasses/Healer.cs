using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PlayerAttack //���ݼӵ� - ������ -
{
    public Syringe attackPrefab;
    public GameObject ultimateAttackPrefab;

    public override void Attack()
    {
        base.Attack();

        Instantiate(attackPrefab, transform.position, transform.rotation);
    }
}
