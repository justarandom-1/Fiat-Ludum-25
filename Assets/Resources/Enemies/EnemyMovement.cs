using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;


public class EnemyMovement : GameEntity
{
    [SerializeField] protected float speed;
    protected bool isMoving;

    protected Transform Base;

    protected float nextWayPointDistance = 1f;

    protected Path path;
    protected int currentWaypoint = 0;
    bool reachedTarget = false;

    Seeker seeker;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();

        seeker = GetComponent<Seeker>();

        isMoving = true;

        Base = LevelManager.Base.transform;

        UpdatePath();
        
    }

    void UpdatePath()
    {
        seeker.StartPath(rb.position, Base.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    new protected void kill()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if the enemy reaches the tower, stop moving
        // if (collision.collider.CompareTag("Tower"))
        // {
        //     isMoving = false;
        // }

        if (collision.collider.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }
    }

    float VectorToAngle(Vector2 v)
    {
        if(v.magnitude == 0) return 0;

        float r = Mathf.Asin(v.y / v.magnitude) * 180 / Mathf.PI;

        Debug.Log(r);

        if(v.x < 0)
            return 180 - r;

        return r ;            
    }


    void FixedUpdate()
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

            transform.rotation = Quaternion.identity;

            transform.Rotate(0, 0, VectorToAngle(direction) - 90);

            if (distance < nextWayPointDistance)
                currentWaypoint++;
        }
    }
}
