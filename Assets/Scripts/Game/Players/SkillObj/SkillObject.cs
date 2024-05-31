using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObject : MonoBehaviour
{
    protected PlayerInfo owner;
    protected int damage;

    public virtual void InitAndShot(PlayerInfo owner, int damage, float moveSpeed)
    {
        this.owner = owner;
        this.damage = damage;
    }
}
