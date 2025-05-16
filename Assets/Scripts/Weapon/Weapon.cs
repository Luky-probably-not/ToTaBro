using UnityEngine;
using System.Collections;
using TMPro;
using static UnityEngine.Rendering.DebugUI;
using static System.Math;

public abstract class Weapon : MonoBehaviour
{
    public string Name { get; set; }
    public float fireRate { get; set; }
    public float bulletSpeed { get; set; }
    [SerializeField] public GameObject bullet;
    public Transform player { get; set; } = null;
    public bool isWielded { get; set; } = false;
    public float lastShotTime { get; set; } = -Mathf.Infinity;
    public float bulletDisparitionTime { get; set; }
    public int bulletDamage { get; set; }

    public float baseDamage { get; set; }
    public int level { get; set; }

    public float hueOffSet {  get; set; }
    public Color currentColor { get; set; }
    public bool isLegendary { get; set; }
    public SpriteRenderer sprite { get; set; }

    public Canvas popupCanvas;
    public TMP_Text nameTMP;
    public TMP_Text damageTMP;
    public TMP_Text fireRateTMP;
    public TMP_Text levelTMP;

    public bool canBuy = false;
    public int cost = 1;
    public void Init(string name, float fireRate, float bulletSpeed, float disparitionTime, int baseDamage)
    {
        this.name = name;
        this.fireRate = fireRate;
        this.bulletSpeed = bulletSpeed;
        this.bulletDisparitionTime = disparitionTime;
        this.baseDamage = baseDamage;

    }

    protected virtual void Awake()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            this.player = playerObj.transform;
        }
        this.sprite = GetComponentInChildren<SpriteRenderer>();
        currentColor = this.sprite.color;
        hueOffSet = Random.value;

        int random = Random.Range(0, 1);
        this.isLegendary = random == 0;
        popupCanvas = GetComponentInChildren<Canvas>();
        popupCanvas.gameObject.SetActive(false);

    }

    protected void Update()
    {
        if (isWielded && player != null)
        {
            transform.position = player.position;
        }
        if (isLegendary)
        {
            LegendaryColor();
        }
    }

    public Bullet Shoot(Vector2 directionShoot, float angle, Quaternion rotateDirection)
    {        
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.SetPositionAndRotation(transform.position, rotateDirection);
        Rigidbody2D newBulletRb = newBullet.GetComponentInChildren<Rigidbody2D>();
        newBulletRb.linearVelocity = directionShoot * this.bulletSpeed;
        newBullet.transform.rotation = Quaternion.Euler(0, 0, rotateDirection.z+angle);
        Bullet bulletObject = newBullet.GetComponent<Bullet>();
        bulletObject.StartedAngle = angle;
        bulletObject.DieAfterSeconds(this.bulletDisparitionTime);
        bulletObject.Damage = bulletDamage;
        return bulletObject;
    }

    public void Equip(Transform player)
    {
        this.player = player;
        GameManager.Instance.removeFromMarchent(this.gameObject);
        this.isWielded = true;
        this.ClosePopup();
        GameManager.Instance.EquipWeapon(this.gameObject);
    }

    public void Desequip()
    {
        this.player = null;
        this.isWielded = false;
        GameManager.Instance.SetDroppedWeapon(this.gameObject);
    }

    public void LegendaryColor()
    {
        float hue = Mathf.Repeat(Time.time * 0.8f + hueOffSet, 1f);
        Color targetColor = Color.HSVToRGB(hue, 0.8f, 1f);
        currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 10f);
        sprite.color = currentColor;
    }

    public IEnumerator IncreaseFireRate(float time)
    {
        this.fireRate /= 2;
        yield return new WaitForSeconds(time);
        this.fireRate *= 2;
    }
    public abstract void ShootWeapon(Vector2 directionShoot);

    public void ShowPopup()
    {
        if (isWielded) return;
        popupCanvas.gameObject.SetActive(true);
        GetTMP();
        nameTMP.SetText(Name);        
        damageTMP.SetText( bulletDamage.ToString());
        fireRateTMP.SetText((Truncate((1 / fireRate)*10)/10).ToString());
        levelTMP.SetText(level.ToString());
    }

    public void ClosePopup()
    {
        popupCanvas.gameObject.SetActive(false);
    }

    public void GetTMP()
    {
        foreach (TMP_Text tmp in GetComponentsInChildren<TMP_Text>())
        {
            switch (tmp.name)
            {
                case "Name":
                    nameTMP = tmp; break;
                case "Damage":
                    damageTMP = tmp; break;
                case "FireRate":
                    fireRateTMP = tmp; break;
                case "Level":
                    levelTMP = tmp; break;
                default:
                    break;
            }
        }
    }

    public void LevelUp(int level)
    {
        bulletDamage = (int) Round(baseDamage + 1.1f * level);
        this.level = level;
    }
}