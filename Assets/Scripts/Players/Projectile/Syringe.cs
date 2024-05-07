using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : Projectile
{
    public void InflictHeal(PlayerClass player)
    {
        player.TakeHeal(damage);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        /*if (other.CompareTag("Player"))
        {
            PlayerClass target = other.GetComponent<PlayerClass>();
            if(owner.team == target.team)
            {
                InflictHeal(target);
            }
            else
            {
                InflictDamage(target);
            }
        }*/
    }
}
