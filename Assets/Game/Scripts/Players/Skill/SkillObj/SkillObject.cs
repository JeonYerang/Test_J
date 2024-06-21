using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObject : MonoBehaviour
{
    protected Player owner;
    protected int damage;

    public virtual void InitAndShot(Player owner, int damage)
    {
        this.owner = owner;
        this.damage = damage;
    }

    public virtual void GetDamage(Transform target)
    {

    }
}
