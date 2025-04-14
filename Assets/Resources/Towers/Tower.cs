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
    }


    new protected void kill()
    {
        Instantiate(RuinsObject, 
                    transform.position, 
                    transform.rotation);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
