using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSkill : Skill
{
    public string skillAnimation;
    public SkillObject skillPrefab;

    public SkillObject skillObject;
    public bool IsOn { get; set; }

    public override void Init(SkillSet set)
    {
        base.Init(set);

        var castData = (OnOffCastData)set.castData;
        skillAnimation = castData.skillAnimation;
        skillPrefab = castData.skillPrefab;

        if (skillPrefab != null)
            skillObject = Instantiate(skillPrefab,
                owner.transform.position, owner.transform.rotation, owner.transform);
    }

    protected void On()
    {
        skillObject.gameObject.SetActive(true);

        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation);
    }

    protected void Off()
    {
        if (skillObject == null)
            return;

        skillObject.gameObject.SetActive(false);
        owner.SetAnimator("Idle");
    }

    public override void Shot()
    {
        if (IsOn) Off();
        else On();
    }
}