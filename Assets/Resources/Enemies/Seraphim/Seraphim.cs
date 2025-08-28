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

    protected override void FixedUpdate()
    {
        if(Base == null)
            return;

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
