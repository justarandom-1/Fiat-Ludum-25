using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private Transform firePoint;
    [SerializeField] private float cooldown = 1f;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        // if the cooldown period is over, fire a projectile at the tower
        if (timer >= cooldown) 
        {
            Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            timer = 0f;
        }    
    }
}
