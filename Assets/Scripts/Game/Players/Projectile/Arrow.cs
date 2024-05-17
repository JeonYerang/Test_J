using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    int level = 0;
    public GameObject[] particles;

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public override void InitAndShot(PlayerInfo owner, int damage, float moveSpeed)
    {
        this.owner = owner;
        this.damage = damage;

        switch (level)
        {
            case 0:
                Instantiate(particles[level], transform.position, transform.rotation, transform);
                break;
            case 1:
                this.damage *= 2;
                moveSpeed *= 1.2f;
                Instantiate(particles[level], transform.position, transform.rotation, transform);
                break;

            case 2:
                this.damage *= 4;
                moveSpeed *= 1.5f;
                Instantiate(particles[level], transform.position, transform.rotation, transform);
                break;

            default:
                break;
        }

        rb.velocity = -transform.up * moveSpeed;
    }
}
