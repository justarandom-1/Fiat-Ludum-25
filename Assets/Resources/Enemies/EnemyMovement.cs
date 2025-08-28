using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;


public class EnemyMovement : GameEntity
{
    [SerializeField] protected float speed;
    [SerializeField] protected int value;
    
    public bool isActive = false;
    protected bool isMoving;

    protected Transform Base;

    protected float nextWayPointDistance = 1f;

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool reachedTarget = false;

    protected Seeker seeker;
    protected Animator animator;

    [SerializeField] bool lr;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        isActive = true;

        deathSound = Resources.Load<AudioClip>("SFX/angelDeathSound");

        seeker = GetComponent<Seeker>();

        isMoving = true;

        if(LevelManager.instance.Base != null)
            Base = LevelManager.instance.Base.transform;

        animator = GetComponent<Animator>();

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

    protected float VectorToAngle(Vector2 v)
    {
        if(v.magnitude == 0) return 0;

        float r = Mathf.Asin(v.y / v.magnitude) * 180 / Mathf.PI;

        if(v.x < 0)
            return 180 - r;

        return r ;            
    }


    protected override void kill()
    {
        LevelManager.instance.AddSouls(value);

        value = 0;
        
        base.kill();
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isActive)
            return;

        if (collision.collider.CompareTag("Environment"))
        {
            Vector2 force = ((Vector2)transform.position - collision.contacts[0].point).normalized;
            rb.AddForce(force * 50);
        }
        
        if (collision.collider.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
        }

        if (collision.collider.CompareTag("Tower"))
        {
            collision.gameObject.GetComponent<TowerController>().takeDamage(power);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(!isActive)
            return;

        if (other.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(other, GetComponent<Collider2D>());
        }

        if (other.CompareTag("Tower"))
        {
            other.gameObject.GetComponent<TowerController>().takeDamage(power);
        }
    }


    public virtual void Push(Vector2 other)
    {
        LevelManager.instance.PlaySound(hitSFX, 0.4f);
        Vector2 force = ((Vector2)transform.position - other).normalized;
        rb.AddForce(force * 150);
        UpdatePath();
    }


    protected virtual void FixedUpdate()
    {

        if(lr && Base != null)
        {
            float xDirection = Base.position.x - transform.position.x;
            xDirection = xDirection / Mathf.Abs(xDirection);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1 * xDirection, transform.localScale.y, transform.localScale.z);
        }

        // if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn"))
        // {
        //     return;
        // }

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


            if(!lr)
            {
                transform.rotation = Quaternion.identity;
                transform.Rotate(0, 0, VectorToAngle(direction) - 90);
            }
        }
    }
}
