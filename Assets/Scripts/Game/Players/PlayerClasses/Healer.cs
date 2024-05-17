using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PlayerAttack //���ݼӵ� - ������ -
{
    public Syringe attackPrefab;
    public GameObject ultimateAttackPrefab;

    public override void Attack() //�Ʊ��� ������ ��, ������ ������ ��
    {
        base.Attack();

        Projectile projectile = Instantiate(attackPrefab);
        projectile.InitAndShot(GetComponent<PlayerInfo>(), attackPoint, attackSpeed);
    }

    public override void UltimateAttack() //�������� �� or ��
    {
        base.UltimateAttack();
    }
}
