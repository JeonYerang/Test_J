using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerAttack //���ݼӵ� - ������ +
{
    private int comboCount;

    public override void Attack() //�޺�����
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
