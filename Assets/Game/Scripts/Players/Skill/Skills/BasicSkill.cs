using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class BasicSkill : Skill
{
    private string skillAnimation;
    private SkillObject skillPrefab;

    public override void Init(SkillData skillData)
    {
        base.Init(skillData);

        var castData = (BasicCastData)skillData.castData;

        skillAnimation = castData.skillAnimation;
        skillPrefab = castData.skillPrefab;
    }

    public override void Shot()
    {
        int shotDamage = damage;

        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation);

        if (skillPrefab != null)
            Instantiate(skillPrefab, owner.transform.position, owner.transform.rotation);
    }
}
