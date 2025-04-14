using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int power;
    protected AudioClip deathSound;
    protected int curHealth = -1;
    protected Rigidbody2D rb;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(curHealth == -1) curHealth = maxHealth;
    }
    public virtual void takeDamage(int dmg)
    {
        curHealth = Mathf.Max(0, curHealth - dmg);
        if(curHealth == 0)
            this.kill();
    }

    public void setHealth(int h)
    {
        curHealth = h;
    }

    public int GetHealth()
    {
        return curHealth;
    }

    protected virtual void kill()
    {
        LevelManager.instance.PlaySound(deathSound);
        Destroy(gameObject);
    }

    public float getHealth()
    {
        return (float)curHealth / maxHealth;
    }
}