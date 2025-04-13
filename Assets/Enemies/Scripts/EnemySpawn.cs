using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float xPos;
    [SerializeField] private float yPos;
    private Vector3 spawnPosition;
    [SerializeField] private float cooldown = 8f;
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        xPos = 8.2f;
        yPos = 0f;
        spawnPosition = new Vector3(xPos, yPos, 0);
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            timer = 0f;
        }
    }
}
