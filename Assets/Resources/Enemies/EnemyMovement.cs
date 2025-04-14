using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;


public class EnemyMovement : GameEntity
{
    [SerializeField] protected float speed;
    [SerializeField] protected int value;
    protected bool isMoving;

    protected Transform Base;

    protected float nextWayPointDistance = 1f;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachedTarget = false;

    protected Seeker seeker;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        deathSound = Resources.Load<AudioClip>("SFX/angelDeathSound");

        seeker = GetComponent<Seeker>();

        isMoving = true;

        if(LevelManager.instance.Base != null)
            Base = LevelManager.instance.Base.transform;

        UpdatePath();
        
    }

    protected void UpdatePath()
    {
        if(Base == null) return;

        seeker.StartPath(rb.position, Base.position, OnPathComplete);
    }

    protected void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
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


    protected virtual void FixedUpdate()
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

            transform.rotation = Quaternion.identity;

            transform.Rotate(0, 0, VectorToAngle(direction) - 90);
        }
    }
}
