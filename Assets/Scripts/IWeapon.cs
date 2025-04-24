using UnityEngine;
using System.Collections;

public interface IWeapon
{
    string name { get; }
    float fireRate { get; set; }
    float bulletSpeed { get; }
    GameObject bullet { get; set; }
    Transform player { get; set; }
    bool isWielded { get; set; }
    float lastShotTime { get; set; }
    float bulletDisparitionTime { get; set; }

    float hueOffSet { get; set; }
    Color currentColor { get; set; }
    bool isLegendary { get; set; }
    SpriteRenderer sprite {  get; set; }


    public void Shoot(Vector2 directionShoot, float angle, Quaternion rotateDirection, Transform positionWeapon, GameObject newBullet)
    {
        newBullet.transform.SetPositionAndRotation(positionWeapon.position, rotateDirection);
        Rigidbody2D newBulletRb = newBullet.GetComponentInChildren<Rigidbody2D>();
        newBulletRb.linearVelocity = directionShoot * this.bulletSpeed;
        newBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        IBullet bulletObject = newBullet.GetComponent<IBullet>();
        bulletObject.StartedAngle = angle;
        bulletObject.DieAfterSeconds(this.bulletDisparitionTime);
    }

    void ShootWeapon(Vector2 directionShoot) { }

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
