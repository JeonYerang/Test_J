using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PlayerAttack //공격속도 - 데미지 -
{
    public Syringe attackPrefab;

    public override void Attack() //아군이 맞으면 힐, 적군이 맞으면 딜
    {
        Projectile projectile = Instantiate(attackPrefab);
        projectile.InitAndShot(GetComponent<PlayerInfo>(), attackPoint, attackSpeed);
    }

    public override void UltimateAttack() //넓은범위 힐 or 딜
    {

    }
}
