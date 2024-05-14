using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerAttack
{
    public Arrow attackPrefab;

    public override void Attack()
    {
        Projectile projectile = Instantiate(attackPrefab);
        projectile.InitAndShot(GetComponent<PlayerInfo>(), attackPoint, attackSpeed);
    }
}
