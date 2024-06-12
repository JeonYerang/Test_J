using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSkill : Skill
{
    public string skillAnimation;
    public SkillObject skillPrefab;

    public override void Shot()
    {
        Instantiate(skillPrefab, owner.transform.position, owner.transform.rotation);
        owner.SetAnimator(skillAnimation);
    }
}
