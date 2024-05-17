using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerAttack //공격속도 + 데미지 -
{
    public Arrow attackPrefab;
    public GameObject ultimateAttackPrefab;

    public int chargeCount;

    /*protected override void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Charge();
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            EndCharge();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            UltimateAttack();
        }
    }*/

    public void Charge()
    {
        StartCoroutine(ChargingCoroutine());
    }

    public void EndCharge()
    {
        StopCoroutine(ChargingCoroutine());
        Attack();
    }

    private IEnumerator ChargingCoroutine()
    {
        while(chargeCount < 3)
        {
            yield return new WaitForSeconds(2f);
            chargeCount++;
        }
    }

    public override void Attack() //차지공격
    {
        base.Attack();

        int damage = attackPoint;
        float speed = attackSpeed;

        Arrow arrow = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        arrow.SetLevel(chargeCount);
        arrow.InitAndShot(GetComponent<PlayerInfo>(), damage, speed);

        chargeCount = 0;
    }

    public override void UltimateAttack() //연속공격
    {
        base.UltimateAttack();
    }
}
