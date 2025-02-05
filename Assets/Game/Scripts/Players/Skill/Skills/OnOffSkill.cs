using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSkill : Skill
{
    private string skillAnimation;
    private SkillObject skillPrefab;

    private SkillObject skillObject;
    public bool IsOn { get; private set; }

    private void OnDestroy()
    {
        Destroy(skillObject);
    }

    public override void Init(SkillData skillData)
    {
        base.Init(skillData);

        var castData = (OnOffCastData)skillData.castData;

        skillAnimation = castData.skillAnimation;
        skillPrefab = castData.skillPrefab;

        if (skillPrefab != null)
            skillObject = Instantiate(skillPrefab,
                owner.transform.position, owner.transform.rotation, owner.transform);
    }

    private void On()
    {
        skillObject.gameObject.SetActive(true);

        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation);
    }

    private void Off()
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

    public override void InstantiateEffect(Vector3 shotPos, Quaternion shotDir, int damage, Player target)
    {
        throw new System.NotImplementedException();
    }
}