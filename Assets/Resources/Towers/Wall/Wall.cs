using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : TowerController
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyMovement>().takeDamage(power);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyMovement>().takeDamage(power);
        }
    }
}
