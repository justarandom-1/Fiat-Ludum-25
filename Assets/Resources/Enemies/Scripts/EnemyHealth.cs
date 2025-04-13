using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float curHealth;
    [SerializeField] private float damageDealt = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
    }

    void Update()
    {
        if (curHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if a weapon comes in contact with the enemy, take off 0.5 health
        if (collision.CompareTag("Weapon"))
        {
            curHealth -= damageDealt;
        }
    }
}
