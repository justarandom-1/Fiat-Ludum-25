using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QIBeam : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;

    // private float range;
    private Vector2 direction;

    private Transform[] targets = {null, null, null, null};

    private int state = 0;

    private lr_LineController line;

    void Start()
    {
    }

    public void Initialize (Transform start)
    {

        targets[0] = start;

        var allTowers = FindObjectsByType<TowerController>(FindObjectsSortMode.None);

        for(int i = 0; i < allTowers.Length; i++)
        {
            addToList(allTowers[i].gameObject.transform);
        }

        for(int i = 0; i < 4; i++)
        {
            if(targets[i] == null)
            {
                Transform[] t  = new Transform[i];
                for(int j = 0; i < i; j++)
                    t[j] = targets[j];

                targets = t;
            }
        }

        line.SetUpLine(targets);

        state = 1;

        transform.GetChild(0).gameObject.GetComponent<Vertex>().Initialize(targets[1], this);

    }

    void addToList(Transform t)
    {
        for(int i = 0; i < targets.Length; i++)
        {
            if(targets[i] == null || (t.position - targets[0].position).magnitude < (targets[i].position - targets[0].position).magnitude){

                for(int j = i+1; j < targets.Length; j++)
                {
                    targets[j] = targets[j-1];
                }
                targets[i] = t;
                return;
            }
        }
    }

    public void reachedTarget(Transform next)
    {
        if(next == null)
            Destroy(gameObject);

        state += 1;

        if(state == targets.Length)
            Destroy(gameObject);

        next.gameObject.GetComponent<Vertex>().Initialize(targets[state], this);
    }


    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
