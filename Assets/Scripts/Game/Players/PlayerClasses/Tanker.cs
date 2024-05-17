using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : PlayerAttack //공격속도 - 데미지 -
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
        //부서지는 이펙트
        shield.SetActive(false);
    }

    public override void UltimateAttack() //돌진 후 기절시키기
    {
        base.UltimateAttack();
    }
}
