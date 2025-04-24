using System.Collections;
using UnityEngine;

public class LaserGun : MonoBehaviour, IWeapon
{
    public new string name { get; } = "LaserGun";
    [SerializeField] private GameObject bulletObj;
    public float fireRate { get; set; } = 1f;
    public float bulletSpeed { get; set; } = 0;
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

    public bool isLegendary { get; set; } = false;
    public SpriteRenderer sprite { get; set; }
    public Color currentColor { get; set; }
    public float hueOffSet { get; set; }

    public void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            this.player = playerObj.transform;
        }
        _lastShotTime = -Mathf.Infinity;

        this.sprite = GetComponentInChildren<SpriteRenderer>();
        currentColor = this.sprite.color;
        hueOffSet = Random.value;
    }
    public void Update()
    {
        if (isWielded && player != null)
        {
            transform.position = player.position;
        }
        if (isLegendary)
        {
            (this as IWeapon).LegendaryColor();
        }
    }

    public void ShootWeapon(Vector2 directionShoot)
    {
        if (Time.time < lastShotTime + fireRate) return;
        float angle = Mathf.Atan2(directionShoot.y, directionShoot.x) * Mathf.Rad2Deg;

        lastShotTime = Time.time;
        var positionWeapon = gameObject != null ? gameObject.transform : null;

        GameObject newBullet = Instantiate(bullet);
        (this as IWeapon).Shoot(directionShoot, angle, transform.rotation, positionWeapon, newBullet);
    }
}
