using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class BasicSkill : Skill
{
    protected string skillAnimation;
    protected SkillObject skillPrefab;

    public override void Init(SkillSet set)
    {
        base.Init(set);

        var castData = (BasicCastData)set.castData;
        skillAnimation = castData.skillAnimation;
        skillPrefab = castData.skillPrefab;
    }

    public override void Shot()
    {
        int shotDamage = Damage;

        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation);

        if (skillPrefab != null)
            Instantiate(skillPrefab, owner.transform.position, owner.transform.rotation);
    }
}
