using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : GameEntity
{
    // Start is called before the first frame update
    [SerializeField] GameObject RuinsObject;
    [SerializeField] int type;

    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        deathSound = Resources.Load<AudioClip>("SFX/towerDestruction");
    }

    public override void takeDamage(int dmg, bool leaveRuinIfDead)
    {
        curHealth = Mathf.Max(0, curHealth - dmg);  
        if(curHealth == 0){
            LevelManager.instance.PlaySound(deathSound);
            animator.Play("DestroyTower");
        }
    }

    protected override void kill()
    {
        Instantiate(RuinsObject, 
                    new Vector3(transform.position.x, transform.position.y, -0.9f), 
                    transform.rotation);
        
        base.kill();
    }

    void Update()
    {
        if(!spriteRenderer.enabled)
            Destroy(gameObject);
    }

    public int getType()
    {
        return type;
    }

    public float getX()
    {
        return transform.position.x;
    }

    public float getY()
    {
        return transform.position.y;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy") && (collision.collider.gameObject.GetComponent<EnemyMovement>() != null && collision.collider.gameObject.GetComponent<EnemyMovement>().isActive))
        {
            collision.gameObject.GetComponent<EnemyMovement>().Push(transform.position);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && (other.gameObject.GetComponent<EnemyMovement>() != null && other.gameObject.GetComponent<EnemyMovement>().isActive))
        {
            other.gameObject.GetComponent<EnemyMovement>().Push(transform.position);
        }
    }
}
