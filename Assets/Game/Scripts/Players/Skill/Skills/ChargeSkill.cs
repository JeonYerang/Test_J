using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSkill : Skill
{
    private int maxChargeCount;
    public int CurrentChargeCount { get; private set; }
    public float ChargeInterval { get; private set; }

    public bool IsCharging { get; private set; }

    private string skillAnimation;
    private SkillObject[] skillPrefab;

    private string chargingAnimation;
    private GameObject chargingPrefab;

    public override void Init(SkillSet set)
    {
        base.Init(set);

        var castData = (ChargeCastData)set.castData;
        skillAnimation = castData.skillAnimation;
        skillPrefab = castData.skillPrefab;
        chargingPrefab = castData.ChargingPrefab;
    }

    public void StartCharge()
    {
        IsCharging = true;

        chargeCoroutine = StartCoroutine(ChargeCoroutine());

        if (chargingAnimation != null)
            owner.SetAnimator(chargingAnimation);

        if (chargingPrefab != null)
            Instantiate(chargingPrefab, owner.transform.position, owner.transform.rotation);

    }

    public void EndCharge()
    {
        IsCharging = false;

        if (chargeCoroutine != null)
            StopCoroutine(ChargeCoroutine());

        Shot();
    }

    private Coroutine chargeCoroutine = null;
    private IEnumerator ChargeCoroutine()
    {
        CurrentChargeCount = 0;
        while (CurrentChargeCount < maxChargeCount)
        {
            yield return new WaitForSeconds(ChargeInterval);
            CurrentChargeCount++;
        }
    }

    public override void Shot()
    {
        int shotDamage = Damage * CurrentChargeCount;

        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation);

        if (skillPrefab != null)
            Instantiate(skillPrefab[CurrentChargeCount], owner.transform.position, owner.transform.rotation);
    }
}