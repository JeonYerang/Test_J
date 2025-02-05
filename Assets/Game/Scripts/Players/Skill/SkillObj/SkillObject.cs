using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//다른 팀에게만 공격
public abstract class SkillObject : MonoBehaviour
{
    protected PhotonTeam owningTeam;
    protected Player target;

    protected int damage;

    public virtual void SetObject(PhotonTeam owningTeam, int damage, Player target = null)
    {
        this.owningTeam = owningTeam;
        this.target = target;
        this.damage = damage;
    }

    protected abstract void Release();

    public virtual void TakeDamage(PlayerAttack damageTarget)
    {
        damageTarget.GetDamage(damage);
    }
}
