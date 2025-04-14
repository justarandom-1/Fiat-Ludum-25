using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : GameEntity
{
    // Start is called before the first frame update
    [SerializeField] GameObject RuinsObject;
    new protected void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
