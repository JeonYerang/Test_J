using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingArrow : ChargeSkill
{
    public override void UsingSkill()
    {
        Instantiate(data.skillPrefab, 
            owner.transform.position, owner.transform.rotation);
    }
}

public class ShieldSkill : OnOffSkill
{

}