using System.Collections;
using UnityEngine;

public class LaserGun : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        Name = "Laser";
        fireRate = 2f;
        bulletSpeed = 0;
        bulletDisparitionTime = 0.2f;
        baseDamage = 5;
    }

    public override void ShootWeapon(Vector2 directionShoot)
    {
        if (Time.time < lastShotTime + fireRate) return;
        
        lastShotTime = Time.time;

        float angle = Mathf.Atan2(directionShoot.y, directionShoot.x) * Mathf.Rad2Deg;
        Shoot(directionShoot, angle, transform.rotation);
        if (isLegendary)
        {
            Shoot(directionShoot, angle+20, transform.rotation);
            Shoot(directionShoot, angle-20, transform.rotation);
        }
    }
}