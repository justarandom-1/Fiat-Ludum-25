using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Vector2 dir;

    private int hits = 0;

    private int power = 250;

    [SerializeField] float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();   

        animator = GetComponent<Animator>();

        float angle = Mathf.PI/2 - 2 * Mathf.PI / 9 * int.Parse(gameObject.name);
        dir = (new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))).normalized;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("star"))
        {
            rb.velocity = dir * speed;

            if(!spriteRenderer.isVisible)
                Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower"))
        {
            hits++;

            LevelManager.instance.PlaySound(Resources.Load<AudioClip>("SFX/shieldHit"));

            GameEntity enemy = collision.GetComponent<TowerController>();
            if (enemy != null)
            {
                int dmg = Mathf.Min(power, enemy.getCurrHealth());

                enemy.takeDamage(dmg);
                power -= dmg;
            }

            if(hits >= 3 || power == 0)
                Destroy(gameObject);
        }
    }
}
