using UnityEngine;
using System.Collections;

public interface IWeapon
{
    string name { get; }
    float fireRate { get; }
    float bulletSpeed { get; }
    GameObject bullet { get; set; }
    Transform player { get; set; }
    bool isWielded { get; set; }
    float lastShotTime { get; set; }
    float bulletDisparitionTime { get; set; }
    public void Shoot(Vector2 directionShoot);
}
