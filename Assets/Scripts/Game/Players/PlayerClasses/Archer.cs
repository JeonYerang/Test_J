using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerAttack //공격속도 + 데미지 -
{
    public Arrow attackPrefab;
    public GameObject ultimateAttackPrefab;

    #region Charging
    public int ChargeCount { get; protected set; }
    public void Charging()
    {
        state = AttackState.Charge;
        StartCoroutine(ChargingCoroutine());
    }

    public void EndCharging()
    {
        state = AttackState.Idle;
        StopCoroutine(ChargingCoroutine());
    }

    protected IEnumerator ChargingCoroutine()
    {
        while (Input.GetKeyUp(KeyCode.K))
        {
            yield return new WaitForSeconds(2f);

            if(ChargeCount < 3)
                ChargeCount++;
        }
    }

    public override void Attack()
    {
        base.Attack();

        int damage = attackPoint;
        switch (ChargeCount)
        {
            case 1:
                damage *= 2;
                break;

            case 2:
                damage *= 4;
                break;

            default:
                break;
        }

        Instantiate(attackPrefab, transform.position, transform.rotation);
    }
    #endregion
}

