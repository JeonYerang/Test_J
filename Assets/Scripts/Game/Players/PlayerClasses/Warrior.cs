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
                Instantiate(attackPrefab[0], transform.position, transform.rotation);
                comboCount++;
                comboCoroutine = StartCoroutine(ComboCoroutine());
                break;

            case 1:
                Instantiate(attackPrefab[1], transform.position, transform.rotation);
                comboCount++;
                comboCoroutine = StartCoroutine(ComboCoroutine());
                break;

            case 2:
                Instantiate(attackPrefab[2], transform.position, transform.rotation);
                comboCount = 0;
                break;

            default:
                break;
        }
    }

    Coroutine comboCoroutine = null;
    private IEnumerator ComboCoroutine()
    {
        yield return new WaitForSeconds(3);
        comboCount = 0;
    }
}
