using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    public void ConditionCheck()
    {

    }

    private IEnumerator CoolTimeCoroutine(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}