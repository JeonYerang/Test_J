using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingArrow : Skill, IChargableSkill
{
    public int MaxChargeCount { get; set; }
    public int CurrentChargeCount { get; set; }

    public void Charging()
    {
        StartCoroutine(ChargingCoroutine());
    }

    public void EndCharging()
    {
        StopCoroutine(ChargingCoroutine());
        UsingSkill();
    }

    private IEnumerator ChargingCoroutine()
    {
        while (CurrentChargeCount < 3)
        {
            yield return new WaitForSeconds(2f);
            CurrentChargeCount++;
        }
    }

    public override void UsingSkill()
    {
        throw new System.NotImplementedException();
    }
}
