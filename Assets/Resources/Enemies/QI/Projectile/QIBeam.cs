using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QIBeam : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int damage;

    [SerializeField] AudioClip SFX;

    // private float range;
    private Vector2 direction;

    private Transform[] targets = {null, null, null, null};

    private Transform[] vertices = {null, null, null, null};

    private int state = 0;

    private lr_LineController line;

    List<GameObject> hits = new List<GameObject>();

    void Start()
    {
    }

    public void Initialize (Transform start, int numOGTowers, bool hasted = false)
    {
        line = GetComponent<lr_LineController>();

        if(hasted)
            speed *=2;

        vertices[0] = start;

        vertices[1] = transform.GetChild(0);

        vertices[2] = transform.GetChild(0).GetChild(0);

        vertices[3] = transform.GetChild(0).GetChild(0).GetChild(0);

        targets[0] = start;

        var allTowers = FindObjectsByType<TowerController>(FindObjectsSortMode.None);

        for(int i = 0; i < allTowers.Length; i++)
        {
            if(allTowers[i].gameObject == LevelManager.instance.Base && (allTowers.Length > 3 && (numOGTowers > 1  || Random.Range(0, 2) == 0)))
                continue;
            addToList(allTowers[i].gameObject.transform);
        }

        shortenTargets();

        if(targets.Length == 0)
        {
            Destroy(gameObject);
            return;
        }

        line.SetUpLine(vertices);

        state = 0;

        reachedTarget();

    }

    void addToList(Transform t)
    {
        for(int i = 0; i < 4; i++)
        {
            if(targets[i] == null || (t.position - targets[0].position).magnitude < (targets[i].position - targets[0].position).magnitude){

                for(int j = i+1; j < 4; j++)
                {
                    targets[j] = targets[j-1];
                }
                targets[i] = t;
                return;
            }
        }
    }

    void shortenTargets()
    {
        int i = 0;

        for(; i < 4; i++)
        {
            if(targets[i] == null)
                break;
        }

        Transform[] t  = new Transform[i];
        
        for(int j = 0; j < i; j++)
            t[j] = targets[j];

        targets = t;
    }

    public void reachedTarget(bool terminate = false)
    {
        state += 1;

        if(state >= targets.Length || terminate){
            Destroy(gameObject);
            return;
        }

        LevelManager.instance.PlaySound(SFX);

        vertices[state].gameObject.GetComponent<Vertex>().Initialize(targets[state], this, speed);

        vertices[state].SetParent(transform);
    }


    void FixedUpdate()
    {
        if(targets[0] == null)
            Destroy(gameObject);
    }

    public void hit(GameObject target)
    {
        for(int i = 0; i < hits.Count; i++)
            if(hits[i] != null && hits[i] == target)
                return;
            

        hits.Add(target);
        GameEntity enemy = target.GetComponent<TowerController>();
        if (enemy != null)
        {
            enemy.takeDamage(damage, false);
        }
    }
}
