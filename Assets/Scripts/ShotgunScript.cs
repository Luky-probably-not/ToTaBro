using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour, IWeapon
{

    public string name { get; } = "Shotgun";
    [SerializeField] private GameObject bulletObj;
    public float fireRate { get; set; } = 1f;
    public float bulletSpeed { get; set; } = 20;
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
    public float bulletDisparitionTime { get; set; } = 0.2f;

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

        print($"{Time.time} and {lastShotTime}");
        if (Time.time < lastShotTime + fireRate) return;
        lastShotTime = Time.time;
        int numberBullet = 5;
        float spreadAngle = 20;
        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadAngle / (numberBullet - 1);
        for (float i = 0; i < numberBullet; i++)
        {
            float angle = startAngle + i * angleStep;
            Vector2 rotateDirection = Quaternion.Euler(0, 0, angle) * directionShoot;
            Transform? positionWeapon = gameObject != null ? gameObject.transform : null;

            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.SetPositionAndRotation(positionWeapon.position, Quaternion.LookRotation(Vector3.forward, rotateDirection));
            Rigidbody2D newBulletRb = newBullet.GetComponent<Rigidbody2D>();
            newBulletRb.linearVelocity = rotateDirection.normalized * bulletSpeed;
            Bullet bulletObject = newBullet.GetComponent<Bullet>();
            bulletObject.DieAfterSeconds(bulletDisparitionTime);
        }
    }
}
