using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : TowerController
{
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected float range;

    [SerializeField] float rotationSpeed;

    [SerializeField] float fireRate;

    [SerializeField] AudioClip fireSFX;

    private Transform turret;

    protected Transform barrel;
    private Transform target;

    protected float aim = 90;

    private float timer = 0;

    protected override void Start()
    {
        base.Start();

        turret = transform.GetChild(1);

        barrel = turret.GetChild(0);
    }


    Transform GetTarget()
    {
        var allEnemies = FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);

        Transform t = null;

        float min = 0;

        for(int i = 0; i < allEnemies.Length; i++)
        {
            Transform g = allEnemies[i].gameObject.transform;
            float dist1 = Vector2.Distance(g.position, transform.position);
            float dist2 = Vector2.Distance(g.position, LevelManager.instance.Base.transform.position);
            if(dist1 < range && (t == null || dist2 < min)) 
            {
                t = g;
                min = dist2;
            }
        }

        return t;
    }

    void FixedUpdate()
    {
        if(timer > 0)
            timer = Mathf.Max(timer - Time.deltaTime, 0);

        if(LevelManager.instance.phase != 1 || LevelManager.instance.Base == null)
            return;

        target = GetTarget();

        if(target == null)
            return;

        if(timer == 0)
        {
            FireProjectile();
            LevelManager.instance.PlaySound(fireSFX);
            timer = fireRate;
        }

        float a = Time.deltaTime * rotationSpeed;

        float targetAngle = VectorToAngle(target.position - transform.position);

        if(Mathf.Abs(targetAngle - aim) > 355 || Mathf.Abs(targetAngle - aim) < 5)
            return;
        else if(Mathf.Abs(targetAngle - aim) < 180)
            aim += a * Mathf.Abs(targetAngle - aim) / (targetAngle - aim);
        else
            aim -= a * Mathf.Abs(targetAngle - aim) / (targetAngle - aim);

        // for (float angle = 0; angle < 360f; angle += scanStepAngle)
        // {
        //     Vector2 aim = AngleToVector(angle);
        //     RaycastHit2D hit = Physics2D.Raycast(transform.position, aim, detectionRadius);

        //     if (hit.collider != null && hit.collider.CompareTag("Enemy"))
        //     {
        //         RotateToFace(aim);
        //         FireProjectile(aim);
        //         break; // only shoot at the first enemy detected
        //     }
        // }

        turret.rotation = Quaternion.Euler(0f, 0f, aim - 90);
    }

    protected virtual void FireProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, barrel.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(AngleToVector(aim), range, power);
    }

    protected Vector2 AngleToVector(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
    }

    protected float VectorToAngle(Vector2 v)
    {
        float angle = Mathf.Asin(v.y/v.magnitude) * Mathf.Rad2Deg;
        if(v.x > 0)
            return angle;
        angle = 180 - angle;
        if(angle > 0)
            return angle;
        return 360 + angle;
    }
}