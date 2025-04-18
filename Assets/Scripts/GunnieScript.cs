using System.Collections;
using UnityEngine;

public class Gunnie : MonoBehaviour, IWeapon
{
    public string name { get; } = "Gunnie";
    [SerializeField] private GameObject bulletObj;
    public float fireRate { get; set; } = 0.5f;
    public float bulletSpeed { get; set; } = 7;
    public GameObject bullet
    {
        get => bulletObj;
        set => bulletObj = value;
    }
    public Transform player { get; set; }
    public bool isWielded { get; set; } = false;
    [System.NonSerialized]
    private float _lastShotTime;
    public float lastShotTime
    {
        get => _lastShotTime;
        set => _lastShotTime = value;
    }
    public float bulletDisparitionTime { get; set; } = 1.5f;

    public void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            this.player = playerObj.transform;
        }
        _lastShotTime = -Mathf.Infinity;
    }

    public void Update()
    {
        if (isWielded && player != null)
        {
            transform.position = player.position;
        }
    }
    public void Shoot(Vector2 directionShoot)
    {
        if (Time.time < lastShotTime + fireRate) return;

        lastShotTime = Time.time;
        Transform? positionWeapon = gameObject != null ? gameObject.transform : null;

        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.SetPositionAndRotation(positionWeapon.position, transform.rotation);
        Rigidbody2D newBulletRb = newBullet.GetComponent<Rigidbody2D>();
        newBulletRb.linearVelocity = directionShoot * bulletSpeed;
        Bullet bulletObject = newBullet.GetComponent<Bullet>();
        bulletObject.DieAfterSeconds(bulletDisparitionTime);
    }
}
