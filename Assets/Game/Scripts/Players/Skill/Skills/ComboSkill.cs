using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboSkill : Skill
{
    public int maxComboCount;
    public int currentComboCount { get; set; }

    public string[] skillAnimation;
    public SkillObject[] skillPrefab;

    public override void Init(SkillSet set)
    {
        base.Init(set);

        var castData = (ComboCastData)set.castData;
        skillAnimation = castData.skillAnimation;
        skillPrefab = castData.skillPrefab;
    }

    public override void Shot() //�޺�����
    {
        int shotDamage = Damage * currentComboCount;

        //��ų
        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation[currentComboCount]);

        if (skillPrefab != null)
            Instantiate(skillPrefab[currentComboCount], owner.transform.position, owner.transform.rotation);

        ComboSet();
    }

    protected void ComboSet()
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