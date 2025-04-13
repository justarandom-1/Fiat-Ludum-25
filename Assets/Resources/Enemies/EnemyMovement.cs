using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class GameEntity : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int curHealth;
    protected Rigidbody2D rb;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
    }
    public virtual void takeDamage(int dmg)
    {
        curHealth = Mathf.Max(0, curHealth - dmg);
        if(curHealth == 0)
            kill();
    }

    virtual protected void kill()
    {

    }

    public float getHealth()
    {
        return curHealth / maxHealth;
    }
}

public class EnemyMovement : GameEntity
{
    [SerializeField] protected float speed;
    protected bool isMoving;
    [SerializeField] protected float direction;

   [SerializeField] bool track;

    protected Transform Base;
    protected int turnDirection;
    // Start is called before the first frame update
    new protected void Start()
    {
        base.Start();

        isMoving = true;

        Base = LevelManager.Base.transform;

        direction = directionToBase();

        turnDirection = 0;
    }

    float directionToBase()
    {
        float r = Mathf.Asin((Base.position.y - transform.position.y) / Vector2.Distance(Base.position, transform.position));

        if((Base.position.x - transform.position.x) < 0)
            r = Mathf.PI - r;

        return r;            
    }

    Vector2 directionToVector(float direction)
    {
        return new Vector2(Mathf.Cos(direction), Mathf.Sin(direction));
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


    void Update()
    {
        if (isMoving)
        {
            RaycastHit2D r1 = Physics2D.Raycast((Vector2)transform.position + directionToVector(direction) * 0.5f, directionToVector(direction), 4f);

            RaycastHit2D r2 = Physics2D.Raycast((Vector2)transform.position + directionToVector(direction) * 0.5f, directionToVector(direction + 0.16f), 2.5f);

            RaycastHit2D r3 = Physics2D.Raycast((Vector2)transform.position + directionToVector(direction) * 0.5f, directionToVector(direction - 0.16f), 2.5f);

            float diff = directionToBase() - direction;

            if(r1.collider != null && r1.collider.tag == "Environment")
            {
                if(turnDirection == 0)
                    if(((Vector2)transform.position + directionToVector(direction + 1.5f) - (Vector2)Base.position).magnitude < 
                       ((Vector2)transform.position + directionToVector(direction - 1.5f) - (Vector2)Base.position).magnitude)
                        turnDirection = 1;
                    else
                        turnDirection = -1;

                direction += 5 * Time.deltaTime * turnDirection;

                if(track)
                    Debug.Log("turn (block front)");
            }
            else if(r2.collider != null && r2.collider.tag == "Environment" && (r3.collider == null || r3.collider.tag != "Environment"))
            {
                direction -= 5 * Time.deltaTime;

                if(track)
                   Debug.Log("turn (block left)");
            }
            else if(r3.collider != null && r3.collider.tag == "Environment" && (r2.collider == null || r2.collider.tag != "Environment"))
            {
                direction += 5 * Time.deltaTime;

                if(track)
                    Debug.Log("turn (block right)");

            }
            else
            {
                turnDirection = 0;

                float returnDirection = direction + 3 * Time.deltaTime * diff / Mathf.Abs(diff);

                if(Mathf.Abs(diff) > 0.001f){
                    if(diff < 0)
                        returnDirection = Mathf.Max(returnDirection, directionToBase());
                    else
                        returnDirection = Mathf.Min(returnDirection, directionToBase());

                    RaycastHit2D r4 = Physics2D.Raycast((Vector2)transform.position + directionToVector(returnDirection), directionToVector(returnDirection), 4f);

                    if(r4.collider == null || r4.collider.tag != "Environment"){
                        direction = returnDirection;

                        if(track)
                            Debug.Log("turn (refocus)");
                    }
                }
            }


            transform.rotation = Quaternion.identity;

            transform.Rotate(0, 0, direction * 180 / Mathf.PI - 90);

            rb.velocity = directionToVector(direction) * speed;
        }
    }
}
