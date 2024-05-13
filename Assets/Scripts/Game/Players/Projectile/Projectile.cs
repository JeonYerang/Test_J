using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected PlayerInfo owner;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected float moveSpeed;

    protected Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Reinforce(int plusDamage, float plusSpeed)
    {
        damage += plusDamage;
        moveSpeed += plusSpeed;
    }

    public virtual void Shot(PlayerInfo owner)
    {
        this.owner = owner;
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
