using System.Collections;
using UnityEngine;

public class Sword : Weapon
{
    protected override void Awake()
    {
        base.Awake();
        Name = "Sword";
        fireRate = 1f;
        bulletSpeed = 0;
        bulletDisparitionTime = 0.2f;
        baseDamage = 3;
    }

    public override void ShootWeapon(Vector2 directionShoot)
    {
        if (Time.time < lastShotTime + fireRate) return;

        lastShotTime = Time.time;
        StartCoroutine(CutClean(directionShoot));
        if (isLegendary) StartCoroutine(CutClean(directionShoot, true));
    }

    public IEnumerator CutClean(Vector2 directionShoot, bool legendary = false)
    {
        float direction = 1f;
        if (legendary)
        {
            yield return new WaitForSeconds(0.3f);
            direction = -1f;
        }
        float angle = Mathf.Atan2(directionShoot.y, directionShoot.x) * Mathf.Rad2Deg;
        Bullet slash = Shoot(directionShoot, angle, transform.rotation);
        slash.slash = direction;
    }
}