using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{    
    float speed;

    [SerializeField] int state = 0;

    Rigidbody2D rb;

    Transform target;

    Vector2 targetPos;

    QIBeam parent;

    [SerializeField] Vector2 direction;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Initialize(Transform t, QIBeam p, float s = 15)
    {
        state = 1;
        target = t;
        parent = p;
        speed = s;
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(state == 1)
        {
            if(target != null)
                targetPos = target.position;

            Vector2 distance = targetPos - (Vector2)transform.position;
            direction = distance.normalized;

            rb.velocity = speed * direction;

            if(distance.magnitude < 0.25f){
                transform.position = targetPos;
                state = 2;
                rb.velocity = new Vector2(0, 0);          
                parent.reachedTarget();

                if(target != null)
                {
                    parent.gameObject.GetComponent<QIBeam>().hit(target.gameObject);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if(collision.CompareTag("Environment"))
        //     Destroy(gameObject);

        if (collision.CompareTag("Tower") && parent != null)
        {
            parent.hit(collision.gameObject);
        }
    }

    
}
