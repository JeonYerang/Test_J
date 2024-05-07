using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : PlayerClass
{
    public Syringe attackPrefab;

    public override void Attack()
    {
        Syringe syringe = Instantiate(attackPrefab);
        syringe.InitAndShot(this, attackPoint, attackSpeed);
    }

    public void Resurrection(PlayerClass target)
    {

    }
}
