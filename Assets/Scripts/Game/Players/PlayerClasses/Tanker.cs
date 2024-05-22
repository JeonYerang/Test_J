using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tanker : PlayerAttack //���ݼӵ� - ������ -
{
    public bool isShiledOn;

    public Shield shield;
    public GameObject ultimateAttackPrefab;

    public override void Attack()
    {
        base.Attack();
    }

    public void ShieldOn()
    {
        shield.gameObject.SetActive(true);
    }

    public void ShieldOff()
    {
        shield.gameObject.SetActive(false);
    }
}
