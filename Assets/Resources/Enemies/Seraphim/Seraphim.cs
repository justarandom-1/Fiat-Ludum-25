using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;


public class Seraphim : EnemyMovement
{
    // [SerializeField] protected float speed;
    // [SerializeField] protected int value;
    // protected bool isMoving;

    // protected Transform Base;

    // protected float nextWayPointDistance = 1f;

    // protected Path path;
    // protected int currentWaypoint = 0;
    // bool reachedTarget = false;

    // Seeker seeker;

    float xDirection;

    [SerializeField] protected GameObject projectilePrefab;


    [SerializeField] float fireRate;
    private Transform hand;

    private float timer = 1;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        hand = transform.GetChild(3);        
    }

    protected override void kill()
    {
        LevelManager.instance.AddSouls(value);

        value = 0;
        
        base.kill();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

        if (collision.collider.CompareTag("Tower"))
        {
            Vector3 otherPosition = collision.transform.position;
            Vector2 force = (Vector2)(transform.position - otherPosition).normalized;
            rb.AddForce(force * 100);
            collision.gameObject.GetComponent<TowerController>().takeDamage(power);
        }
    }

    float VectorToAngle(Vector2 v)
    {
        if(v.magnitude == 0) return 0;

        float r = Mathf.Asin(v.y / v.magnitude) * 180 / Mathf.PI;

        if(v.x < 0)
            return 180 - r;

        return r ;            
    }


    protected override void FixedUpdate()
    {
        if(path != null)
        {
            if(currentWaypoint >= path.vectorPath.Count){
                reachedTarget = true;
                return;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWayPointDistance)
                currentWaypoint++;
        }

        if(timer > 0)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);

            if(timer == 0)
            {
                timer = fireRate;
                FireProjectile();

            }
        }

        xDirection = Base.position.x - transform.position.x;
        xDirection = xDirection / Mathf.Abs(xDirection);
        
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1 * xDirection, transform.localScale.y, transform.localScale.z);
    }

    protected void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, hand.position, Quaternion.identity);
    }
}
