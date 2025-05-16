using System.Collections;
using UnityEngine;

public class Shotgun : Weapon
{
    int numberBullet = 5;
    float spreadAngle = 20;
    protected override void Awake()
    {
        base.Awake();
        Name = "Mutli-Arrow";
        fireRate = 1.5f;
        bulletSpeed = 20;
        bulletDisparitionTime = 0.2f;
        baseDamage = 1;
    }

    public override void ShootWeapon(Vector2 directionShoot)
    {
        if (Time.time < lastShotTime + fireRate) return;

        lastShotTime = Time.time;

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (numberBullet - 1);
        StartCoroutine(ThrowBullet(directionShoot, startAngle, angleStep));
        if (isLegendary) StartCoroutine(ThrowBullet(directionShoot, startAngle, angleStep, true));

    }
    public IEnumerator ThrowBullet(Vector2 directionShoot, float startAngle, float angleStep, bool legendary = false)
    {
        if (legendary) yield return new WaitForSeconds(0.2f);
        for (float i = 0; i < numberBullet; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 rotateDirection = Quaternion.Euler(0, 0, angle) * directionShoot;

            Shoot(rotateDirection.normalized, Mathf.Atan2(directionShoot.y, directionShoot.x) * Mathf.Rad2Deg+angle, Quaternion.LookRotation(Vector3.forward, rotateDirection));
        }
        yield return null;
    }
}