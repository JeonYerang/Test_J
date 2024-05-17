using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : PlayerAttack //���ݼӵ� - ������ -
{
    public bool isShiledOn;
    public int shieldHp;

    public GameObject shield;
    public GameObject ultimateAttackPrefab;

    public override void Attack()
    {
        base.Attack();
    }

    public void ShieldOn()
    {
        StartCoroutine(ShieldHpCoroutine());
        shield.SetActive(true);
    }

    public void ShieldOff()
    {
        StopCoroutine(ShieldHpCoroutine());
        shield.SetActive(false);
    }

    private IEnumerator ShieldHpCoroutine()
    {
        while(shieldHp > 0)
        {
            yield return new WaitForSeconds(1f);
            shieldHp--;
        }

        ShieldBreak();
    }

    private void ShieldBreak()
    {
        //�μ����� ����Ʈ
        shield.SetActive(false);
    }

    public override void UltimateAttack() //���� �� ������Ű��
    {
        base.UltimateAttack();
    }
}
