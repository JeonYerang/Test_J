using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PlayerAttack //���ݼӵ� - ������ -
{
    public Syringe attackPrefab;

    public override void Attack() //�Ʊ��� ������ ��, ������ ������ ��
    {
        Projectile projectile = Instantiate(attackPrefab);
        projectile.InitAndShot(GetComponent<PlayerInfo>(), attackPoint, attackSpeed);
    }

    public override void UltimateAttack() //�������� �� or ��
    {

    }
}
