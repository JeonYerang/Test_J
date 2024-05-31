using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : Projectile
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInfo target = other.GetComponent<PlayerInfo>();
            PlayerAttack targetAttack = other.GetComponent<PlayerAttack>();
            if(owner.Team == target.Team)
            {
                targetAttack.TakeHeal(damage);
            }
            else
            {
                targetAttack.GetDamage(damage);
            }
        }
    }
}
