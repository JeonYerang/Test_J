using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PlayerAttack
{
    public Syringe attackPrefab;

    public override void Attack()
    {
        Projectile projectile = Instantiate(attackPrefab);
        projectile.InitAndShot(GetComponent<PlayerInfo>(), attackPoint, attackSpeed);
    }

    public void Resurrection(PlayerAttack target)
    {

    }

    public void DamageBuff()
    {

    }
}