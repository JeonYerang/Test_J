using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerAttack //공격속도 - 데미지 +
{
    private int comboCount;

    public override void Attack() //콤보공격
    {
        attackCount++;

        switch (comboCount)
        {
            case 0:

                comboCount++;
                break;

            case 1:

                comboCount++;
                break;

            case 2:

                comboCount = 0;
                break;

            default:
                break;
        }
    }

    public override void UltimateAttack()
    {

    }
}
