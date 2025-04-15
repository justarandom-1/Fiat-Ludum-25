using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;
    private int damage;

    private float range;
    private Vector2 direction;

    private Vector3 startPos;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Called by Shooter.cs to initialize the direction
    public void Initialize(Vector2 dir, float r, int power)
    {
        direction = dir.normalized;
        range = r;

        rb = GetComponent<Rigidbody2D>();

        startPos = transform.position;

        damage = power;

        rb.velocity = speed * direction;
    }

    void FixedUpdate()
    {

        if((startPos - transform.position).magnitude > range || !spriteRenderer.isVisible)
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.CompareTag("Environment"))
        //     Destroy(gameObject);

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
