using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected PlayerClass owner;
    protected int damage;
    protected float moveSpeed;

    protected Rigidbody rb;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void InitAndShot(PlayerClass owner, int damage, float moveSpeed)
    {
        this.owner = owner;
        this.damage = damage;
        this.moveSpeed = moveSpeed;

        Shot();
    }

    public void Shot()
    {
        rb.velocity = -transform.up * moveSpeed;
    }

    public void InflictDamage(PlayerClass player)
    {
        player.GetDamage(damage);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InflictDamage(other.GetComponent<PlayerClass>());
        }
    }
}
