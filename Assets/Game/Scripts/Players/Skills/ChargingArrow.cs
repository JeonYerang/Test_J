using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingArrow : ChargeSkill
{
    public override void Shot()
    {
        damage *= CurrentChargeCount;

        Instantiate(data.skillPrefab, 
            owner.transform.position, owner.transform.rotation);
    }
}

public class ShieldSkill : OnOffSkill
{
    Shield shield;

    /*protected override void On()
    {
        if (shield == null)
        {
            shield = Instantiate(data.skillPrefab, owner.transform.position, 
            owner.transform.rotation, owner.transform).GetComponent<Shield>();
        }
        else
            shield.gameObject.SetActive(true);
    }

    protected override void Off()
    {
        shield.gameObject.SetActive(false);
    }*/
}

public class ComboAttack : ComboSkill
{
    public override void Shot()
    {
        damage *= currentComboCount;

        //owner.animator.SetTrigger("");

        ComboSet();
    }
}