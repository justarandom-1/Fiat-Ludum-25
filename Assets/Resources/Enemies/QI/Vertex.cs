using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{
    [SerializeField] int depth;
    
    float speed = 20;

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

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(state == 1)
        {
            if(target == null){
                parent.reachedTarget(true);
                return;
            }
            direction = ((Vector2)(target.position - transform.position)).normalized;
            rb.velocity = speed * direction;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.CompareTag("Environment"))
        //     Destroy(gameObject);

        if (collision.CompareTag("Tower") && collision.gameObject.transform == target && state == 1)
        {
            state = 2;

            rb.velocity = new Vector2(0, 0);
            // Try to damage the enemy if it has the takeDamage method
            GameEntity enemy = collision.GetComponent<GameEntity>();
            if (enemy != null)
            {
                enemy.takeDamage(80);
            }

            parent.reachedTarget();
        }
    }
}
