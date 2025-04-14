using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : TowerController
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float scanStepAngle = 15f;

    void Update()
    {
        for (float angle = 0; angle < 360f; angle += scanStepAngle)
        {
            Vector2 direction = AngleToVector(angle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRadius);

            if (hit.collider != null && hit.collider.CompareTag("Enemy"))
            {
                RotateToFace(direction);
                FireProjectile(direction);
                break; // only shoot at the first enemy detected
            }
        }
    }

    private void FireProjectile(Vector2 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(direction);
    }

    private void RotateToFace(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private Vector2 AngleToVector(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }
}