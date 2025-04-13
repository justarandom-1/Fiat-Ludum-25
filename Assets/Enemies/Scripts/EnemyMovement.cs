using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if the enemy reaches the tower, stop moving
        if (collision.CompareTag("Tower"))
        {
            isMoving = false;
        }
    }
}
