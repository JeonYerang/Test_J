using Photon.Pun;
using Photon.Realtime;
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
        //애니메이션
        if (skillAnimation != null)
            owner.SetAnimator(skillAnimation);

        if (skillPrefab != null)
            Instantiate(skillPrefab, owner.transform.position, owner.transform.rotation);

        //InstantiateEffect();
    }

    public override void InstantiateEffect(Vector3 shotPos, Quaternion shotDir, int damage, Player target)
    {
        //스킬 이펙트
        var skillEffect = Instantiate(skillPrefab, shotPos, shotDir);
        //skillEffect.SetObject(, damage);
    }
}
