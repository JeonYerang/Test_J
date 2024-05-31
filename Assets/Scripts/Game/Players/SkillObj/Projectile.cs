using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected PlayerInfo owner;
    protected int damage;
    protected Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitAndShot(PlayerInfo owner, int damage, float moveSpeed)
    {
        this.owner = owner;
        this.damage = damage;
        rb.velocity = -transform.up * moveSpeed;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInfo target = other.GetComponent<PlayerInfo>();
            PlayerAttack targetAttack = other.GetComponent<PlayerAttack>();
            if (owner.Team != target.Team)
            {
                targetAttack.GetDamage(damage);
            }
        }
    }
}
