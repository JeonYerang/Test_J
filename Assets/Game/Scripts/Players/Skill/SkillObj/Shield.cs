using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//상대팀 공격만 제거
public class Shield : MonoBehaviour
{
    public int maxHp;
    public int CurrentHp { get; private set; }

    Coroutine lostHpCoroutine = null;

    private void OnEnable()
    {
        CurrentHp = maxHp;
        lostHpCoroutine = StartCoroutine(LostHpCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(lostHpCoroutine);
    }

    private IEnumerator LostHpCoroutine()
    {
        while(CurrentHp > 0)
        {
            yield return new WaitForSeconds(2);
            CurrentHp--;
        }

        BreakShield();
    }

    public void GetDamage(int damage)
    {
        CurrentHp -= damage;

        if(CurrentHp <= 0)
        {
            CurrentHp = 0;
            BreakShield();
        } 
    }

    public void BreakShield()
    {
        gameObject.SetActive(false); //자기 자신 비활성화 가능?
    }
}
