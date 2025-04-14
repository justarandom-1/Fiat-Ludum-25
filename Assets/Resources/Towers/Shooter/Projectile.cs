using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;

    private Vector2 direction;

    // Called by Shooter.cs to initialize the direction
    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime); // Auto-destroy after a few seconds
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Try to damage the enemy if it has the takeDamage method
            GameEntity enemy = collision.GetComponent<GameEntity>();
            if (enemy != null)
            {
                enemy.takeDamage(damage);
            }

            Destroy(gameObject); // Destroy the projectile after hitting something
        }
    }
}
