using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSkill : Skill
{
    private int maxChargeCount;
    public int CurrentChargeCount { get; private set; }
    private float chargeInterval;

    public bool IsCharging { get; private set; }

    private string skillAnimation;
    private SkillObject[] skillPrefabs;

    private string chargingAnimation;
    private GameObject chargingPrefab;

    public override void Init(SkillData skillData)
    {
        base.Init(skillData);

        var castData = (ChargeCastData)skillData.castData;

        maxChargeCount = castData.maxChargeCount;
        chargeInterval = castData.chargeInterval;

        skillAnimation = castData.skillAnimation;
        skillPrefabs = castData.skillPrefabs;
        chargingAnimation = castData.chargingAnimation;
        chargingPrefab = castData.ChargingPrefab;

        IsCharging = false;
    }

    GameObject chargingObject = null;
    public void StartCharge()
    {
        IsCharging = true;
        chargeCoroutine = StartCoroutine(ChargeCoroutine());

        if (chargingAnimation != null)
            owner.SetAnimator(chargingAnimation);

        if (chargingPrefab != null)
            chargingObject 
                = Instantiate(chargingPrefab, owner.transform.position, owner.transform.rotation);

    }

    public void EndCharge()
    {
        IsCharging = false;
        if (chargeCoroutine != null)
            StopCoroutine(ChargeCoroutine());

        Destroy(chargingObject);
    }

    private Coroutine chargeCoroutine = null;
    private IEnumerator ChargeCoroutine()
    {
        CurrentChargeCount = 0;
        while (CurrentChargeCount < maxChargeCount)
        {
            yield return new WaitForSeconds(chargeInterval);
            CurrentChargeCount++;
        }
    }

    public override void Shot()
    {
        int shotDamage = damage * CurrentChargeCount;

        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation);

        if (skillPrefabs != null)
            Instantiate(skillPrefabs[CurrentChargeCount], owner.transform.position, owner.transform.rotation);
    }
}