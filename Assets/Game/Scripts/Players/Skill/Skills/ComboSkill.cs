using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSkill : Skill
{
    private int maxComboCount;
    public int currentComboCount { get; private set; }

    private string[] skillAnimation;
    private SkillObject[] skillPrefab;

    public override void Init(SkillSet set)
    {
        base.Init(set);

        var castData = (ComboCastData)set.castData;
        skillAnimation = castData.skillAnimation;
        skillPrefab = castData.skillPrefab;
    }

    public override void Shot() //콤보공격
    {
        int shotDamage = Damage * currentComboCount;

        //스킬
        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation[currentComboCount]);

        if (skillPrefab != null)
            Instantiate(skillPrefab[currentComboCount], owner.transform.position, owner.transform.rotation);

        ComboCheck();
    }

    private void ComboCheck()
    {
        if (comboCoroutine != null) StopCoroutine(comboCoroutine);

        if (currentComboCount < maxComboCount)
        {
            currentComboCount++;
            comboCoroutine = StartCoroutine(ComboCoroutine());
        }
        else
            currentComboCount = 0;
    }

    Coroutine comboCoroutine = null;
    private IEnumerator ComboCoroutine()
    {
        yield return new WaitForSeconds(3);
        currentComboCount = 0;
    }
}
