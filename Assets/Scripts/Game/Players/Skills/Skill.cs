using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public string _name;
    public int damage;
    public float coolTime;

    public abstract void UseSkill();
}
