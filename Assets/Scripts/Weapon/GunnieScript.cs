using System.Collections;
using UnityEngine;

public class Gunnie : Weapon
{
        protected override void Awake()
    {
        base.Awake();
        Name = "Gunnie";
        fireRate = 0.5f;
        bulletSpeed = 7;
        bulletDisparitionTime = 1.5f;
        baseDamage = 2;
    }

    public override void ShootWeapon(Vector2 directionShoot)
    {
        if (Time.time < lastShotTime + fireRate) return;

        lastShotTime = Time.time;
        Shoot(directionShoot, 0, transform.rotation);
    }
}