using System.Collections;
using System.Collections.Generic;
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