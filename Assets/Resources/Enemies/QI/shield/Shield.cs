using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : GameEntity
{
    // Start is called before the first frame update
    private Animator animator;
    private int state = 0;
    protected override void Start()
    {
        base.Start();

        animator = GetComponent<Animator>();

    }

    protected override void kill()
    {
        state = 2;
        LevelManager.instance.PlaySound(hitSFX, 0.4f);
        animator.Play("shieldDown");
    }

    public int getState()
    {
        return state;
    }

    public void activate()
    {
        state = 1;
        animator.Play("shieldUp");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Wall wall = other.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            LevelManager.instance.PlaySound(hitSFX, 0.4f);
            int dmg = Mathf.Min(curHealth, wall.getCurrHealth());
            other.gameObject.GetComponent<Wall>().takeDamage(dmg, false);
            takeDamage(dmg);
        }
    }

}
