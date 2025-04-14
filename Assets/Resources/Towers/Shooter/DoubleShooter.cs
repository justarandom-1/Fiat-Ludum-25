using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShooter : Shooter
{
    private Transform barrel1;

    private Transform barrel2;

    protected override void Start()
    {
        base.Start();

        barrel1 = transform.GetChild(1).GetChild(1);

        barrel2 = transform.GetChild(1).GetChild(2);
    }
    protected override void FireProjectile()
    {
        GameObject projectile1 = Instantiate(projectilePrefab, barrel1.position, Quaternion.identity);
        projectile1.GetComponent<Projectile>().Initialize(AngleToVector(aim), range, power);

        GameObject projectile2 = Instantiate(projectilePrefab, barrel2.position, Quaternion.identity);
        projectile2.GetComponent<Projectile>().Initialize(AngleToVector(aim), range, power);
    }
}
