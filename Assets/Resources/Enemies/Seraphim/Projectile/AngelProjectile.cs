using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelProjectile : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;

    // private float range;
    private Vector2 direction;

    private Vector3 startPos;

    private Rigidbody2D rb;
    private Animator animator;

    private bool hasExploded;

    [SerializeField] AudioClip explosionSFX;

    void Start()
    {
        Vector2 targetPos = GetTarget();

        transform.Rotate(new Vector3(0, 0, Random.Range(0, 360)));

        direction = targetPos - (Vector2) transform.position;

        rb = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();

        startPos = transform.position;

        rb.velocity = speed * direction;
    }

    Vector2 GetTarget()
    {
        var allTowers = FindObjectsByType<TowerController>(FindObjectsSortMode.None);

        Transform t = null;

        float min = 0;

        for(int i = 0; i < allTowers.Length; i++)
        {
            Transform g = allTowers[i].gameObject.transform;
            float dist = Vector2.Distance(g.position, transform.position);

            if(t == null || dist < min)
            {
                t = g;
                min = dist;
            }
        }

        if(t == null) Destroy(gameObject);

        return (Vector2)t.position;
    }

    void FixedUpdate()
    {
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Done")) //startPos - transform.position).magnitude > range
            Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.CompareTag("Environment"))
        //     Destroy(gameObject);

        if (collision.CompareTag("Tower"))
        {
            if(!hasExploded)
            {
                LevelManager.instance.PlaySound(explosionSFX);
                animator.Play("Explode");
                hasExploded = true;
                rb.velocity = new Vector2(0, 0);
            }

            // Try to damage the enemy if it has the takeDamage method
            GameEntity enemy = collision.GetComponent<GameEntity>();
            if (enemy != null)
            {
                enemy.takeDamage(damage);
            }
        }
    }
}
