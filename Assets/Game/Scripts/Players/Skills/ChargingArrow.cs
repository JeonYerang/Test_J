using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingArrow : ChargeSkill
{
    public override void UsingSkill()
    {
        damage *= CurrentChargeCount;

        Instantiate(data.skillPrefab, 
            owner.transform.position, owner.transform.rotation);
    }
}

public class ShieldSkill : OnOffSkill
{
    GameObject skillObj;

    protected override void On()
    {
        if (skillObj == null)
        {
            skillObj = Instantiate(data.skillPrefab,
            owner.transform.position, owner.transform.rotation, owner.transform);
        }
        else
            skillObj.gameObject.SetActive(true);
    }

    protected override void Off()
    {
        skillObj.gameObject.SetActive(false);
    }
}

public class ComboAttack : ComboSkill
{
    public override void UsingSkill()
    {
        damage *= currentComboCount;

        //owner.animator.SetTrigger("");

        ComboCheck();
    }
}