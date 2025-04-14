using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : GameEntity
{
    // Start is called before the first frame update
    [SerializeField] GameObject RuinsObject;
    [SerializeField] int type;
    protected override void Start()
    {
        base.Start();

        deathSound = Resources.Load<AudioClip>("SFX/towerDestruction");
    }

    protected override void kill()
    {
        Instantiate(RuinsObject, 
                    transform.position, 
                    transform.rotation);
        
        base.kill();
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
}
