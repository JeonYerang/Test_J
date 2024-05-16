using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerAttack //공격속도 + 데미지 -
{
    public Arrow attackPrefab;
    public int chargeCount;

    public void Charge()
    {

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
        attackCount++;

        int damage = attackPoint;
        float speed = attackSpeed;

        switch (chargeCount)
        {
            case 0:
                
                break;

            case 1:
                damage *= 2;
                speed *= 1.2f;
                break;

            case 2:
                damage *= 4;
                speed *= 1.5f;
                break;

            default:
                break;
        }

        Projectile projectile = Instantiate(attackPrefab);
        projectile.InitAndShot(GetComponent<PlayerInfo>(), damage, speed);

        chargeCount = 0;
    }

    public override void UltimateAttack() //연속공격
    {
        attackCount = 0;
    }
}
