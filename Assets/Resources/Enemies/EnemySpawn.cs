using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct SpawnPattern
{
    public SpawnPattern(int s, int t, int n, float r)
    {
        source = "EnemySpawner (" + s.ToString() + ")";
        type = t;
        num = n;
        rate = r;
    }
    public string source;
    public int type;
    public int num;
    public float rate;
}

public class EnemySpawn : MonoBehaviour
{
    

    [SerializeField] List<GameObject> enemyPrefabs;
    private Queue<SpawnPattern> spawnQueue = new Queue<SpawnPattern>();
    private SpawnPattern currentPattern;
    [SerializeField] float timer;

    public void addToQueue(SpawnPattern s)
    {
        spawnQueue.Enqueue(s);
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer == 0 && currentPattern.num == 0 && spawnQueue.Count > 0){
            currentPattern = spawnQueue.Dequeue();
        }
        if(timer > 0)
            timer = Mathf.Max(timer - Time.deltaTime, 0);
        else
            if(currentPattern.num > 0 && enemyPrefabs[currentPattern.type] != null){
                Instantiate(enemyPrefabs[currentPattern.type], 
                            new Vector3(transform.position.x, transform.position.y, -1), 
                            Quaternion.identity * enemyPrefabs[currentPattern.type].transform.localRotation);
                currentPattern.num--;
                
                if(currentPattern.num > 0)
                    timer = currentPattern.rate;
                
                // Debug.Log("");
            }
    }
}
