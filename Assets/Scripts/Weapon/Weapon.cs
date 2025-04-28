using System.Collections;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string name { get; }
    public float fireRate { get; set; }
    public float bulletSpeed { get; }
    public GameObject bullet { get; set; }
    public Transform player { get; set; }
    public bool isWielded { get; set; }
    public float lastShotTime { get; set; }
    public float bulletDisparitionTime { get; set; }
    public int bulletDamage { get; set; }

    public float hueOffSet { get; set; }
    public Color currentColor { get; set; }
    public bool isLegendary { get; set; }
    public SpriteRenderer sprite { get; set; }


    public void Shoot(Vector2 directionShoot, float angle, Quaternion rotateDirection)
    {

        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.SetPositionAndRotation(transform.position, rotateDirection);
        Rigidbody2D newBulletRb = newBullet.GetComponentInChildren<Rigidbody2D>();
        newBulletRb.linearVelocity = directionShoot * this.bulletSpeed;
        newBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        IBullet bulletObject = newBullet.GetComponent<IBullet>();
        bulletObject.StartedAngle = angle;
        bulletObject.DieAfterSeconds(this.bulletDisparitionTime);
        bulletObject.Damage = bulletDamage;
    }

    public abstract void ShootWeapon(Vector2 directionShoot);
    public void Equip(Transform player)
    {
        this.player = player;
        this.isWielded = true;
    }

    public void Desequip()
    {
        this.player = null;
        this.isWielded = false;
    }

    public void LegendaryColor()
    {
        float hue = Mathf.Repeat(Time.time * 0.8f + hueOffSet, 1f);
        Color targetColor = Color.HSVToRGB(hue, 0.8f, 1f);
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 10f); // 10f = rapidité de lissage
        sprite.color = currentColor;
    }

    public IEnumerator IncreaseFireRate(float time)
    {
        this.fireRate /= 2;
        yield return new WaitForSeconds(time);
        this.fireRate *= 2;
    }
}