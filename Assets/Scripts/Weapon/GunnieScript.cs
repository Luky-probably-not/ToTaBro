using System.Collections;
using UnityEngine;

public class Gunnie : Weapon
{
        protected override void Awake()
    {
        base.Awake();
        Name = "Bow";
        fireRate = 0.5f;
        bulletSpeed = 7;
        bulletDisparitionTime = 1.5f;
        baseDamage = 2;
    }

    public override void ShootWeapon(Vector2 directionShoot)
    {
        if (Time.time < lastShotTime + fireRate) return;

        lastShotTime = Time.time;
        Shoot(directionShoot, Mathf.Atan2(directionShoot.y, directionShoot.x) * Mathf.Rad2Deg, transform.rotation);
    }
}