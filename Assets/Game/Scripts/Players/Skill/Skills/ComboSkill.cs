using System.Collections;
using System.Linq;
using UnityEngine;

public class ComboSkill : Skill
{
    private int maxComboCount;
    public int CurrentComboCount { get; private set; }

    private string[] skillAnimations;
    private SkillObject[] skillPrefabs;

    public override void Init(SkillData skillData)
    {
        base.Init(skillData);

        var castData = (ComboCastData)skillData.castData;

        maxComboCount = castData.maxComboCount;

        skillAnimations = castData.skillAnimations;
        skillPrefabs = castData.skillPrefabs;
    }

    public override void Shot() //콤보공격
    {
        int shotDamage = damage * CurrentComboCount;

        //스킬
        if (skillAnimations.Length > CurrentComboCount)
            owner.SetAnimator(skillAnimations[CurrentComboCount]);

        if (skillPrefabs.Length > CurrentComboCount)
            Instantiate(skillPrefabs[CurrentComboCount], owner.transform.position, owner.transform.rotation);

        ComboCheck();
    }

    private void ComboCheck()
    {
        if (comboCoroutine != null) StopCoroutine(comboCoroutine);

        if (CurrentComboCount < maxComboCount)
        {
            CurrentComboCount++;
            comboCoroutine = StartCoroutine(ComboCoroutine());
        }
        else
            CurrentComboCount = 0;
    }

    Coroutine comboCoroutine = null;
    private IEnumerator ComboCoroutine()
    {
        yield return new WaitForSeconds(3);
        CurrentComboCount = 0;
    }
}
