using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    [SerializeField] int depth;
    
    float speed = 10;

    int state = 0;

    Rigidbody2D rb;

    Transform target;

    QIBeam parent;

    Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(Transform t, QIBeam p)
    {

        state = 1;
        target = t;
        parent = p;

        direction = ((Vector2)(target.position - transform.position)).normalized;

        rb = GetComponent<Rigidbody2D>();

        rb.velocity = speed * direction;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.CompareTag("Environment"))
        //     Destroy(gameObject);

        if (collision.CompareTag("Tower") && state == 1)
        {
            // Try to damage the enemy if it has the takeDamage method
            GameEntity enemy = collision.GetComponent<GameEntity>();
            if (enemy != null)
            {
                enemy.takeDamage(80);
            }

            parent.reachedTarget(transform.GetChild(0));
        }
    }
}
