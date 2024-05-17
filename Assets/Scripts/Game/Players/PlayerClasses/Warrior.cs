using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerAttack //���ݼӵ� - ������ +
{
    public GameObject[] attackPrefab; //�ִϸ��̼� + ��ƼŬ��?
    public GameObject ultimateAttackPrefab;

    private int comboCount;

    public override void Attack() //�޺�����
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
