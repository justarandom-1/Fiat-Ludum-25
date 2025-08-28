using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : TowerController
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        if (collision.collider.CompareTag("Enemy") && (collision.collider.gameObject.GetComponent<EnemyMovement>() != null && collision.collider.gameObject.GetComponent<EnemyMovement>().isActive))
        {
            collision.gameObject.GetComponent<GameEntity>().takeDamage(power);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.CompareTag("Enemy") && (other.gameObject.GetComponent<EnemyMovement>() != null && other.gameObject.GetComponent<EnemyMovement>().isActive))
        {
            other.gameObject.GetComponent<GameEntity>().takeDamage(power);
        }
    }
}
