using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerAttack //공격속도 - 데미지 +
{
    public GameObject[] attackPrefab; //애니메이션 + 파티클로?
    public GameObject ultimateAttackPrefab;

    private int comboCount;

    public override void Attack() //콤보공격
    {
        base.Attack();

        switch (comboCount)
        {
            case 0:
                GameObject attack = Instantiate(attackPrefab[0]);
                comboCount++;
                break;

            case 1:
                GameObject attack2 = Instantiate(attackPrefab[0]);
                comboCount++;
                break;

            case 2:
                GameObject attack3 = Instantiate(attackPrefab[0]);
                comboCount = 0;
                break;

            default:
                break;
        }
    }

    public override void UltimateAttack()
    {
        base.UltimateAttack();
    }
}
