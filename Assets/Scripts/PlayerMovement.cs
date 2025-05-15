#nullable enable
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using System.Xml.Linq;
using static System.Math;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5;
    private Vector2 directionDash;

    private bool canDash = true;
    private bool isDashing = false;
    private float dashPower = 25f;
    private float dashTime = 0.3f;
    private float dashCooldown = 1f;

    private Weapon? weapon; 
    private bool startedShoot = false;
    private Vector2 directionShoot;

    private bool isNearWeapon = false;
    private Weapon? weaponNear;

    private bool isNearPotion = false;
    private Potion? potionNear;

    [SerializeField] private float LifePoint = 10f;
    private float MaxLifePoint = 10f;
    private bool canBeDamaged = true;

    private int goldValue = 1;
    public int money = 0;

    private int currentLvl = 0;
    private float xpNeeded = 100;
    [SerializeField] private float currentXp = 0;
    private int xpValue = 1;

    public Canvas popupCanvas;
    public TMP_Text textPopUp;

    private SpriteRenderer hpBar;
    private Sprite[] hpSprites;
    private SpriteRenderer xpBar;
    private Sprite[] xpSprites;

    void Start()
    {
        directionShoot = transform.right;
        popupCanvas = GetComponentInChildren<Canvas>();
        popupCanvas.gameObject.SetActive(false);
        hpBar = GameObject.FindGameObjectWithTag("HpBar").GetComponent<SpriteRenderer>();
        xpBar = GameObject.FindGameObjectWithTag("XpBar").GetComponent<SpriteRenderer>();
        hpSprites = Resources.LoadAll<Sprite>("HealthBar");
        xpSprites = Resources.LoadAll<Sprite>("XpBar");
    }

    void Update()
    {
        if (isDashing) return;
        LvlUp();
        HealthBar();
        XpBar();
        if (LifePoint == 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnMove(InputValue value)
    {
        if (isDashing) return;
        Vector2 input = value.Get<Vector2>();
        directionDash = GetDirection(input);
        rb.linearVelocity = new Vector2(input.x * speed, input.y * speed);
    }
    public void OnLook(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        if (input.x != 0 && input.y != 0)
        {
            directionShoot = input.normalized;
        }
    }
    public void OnDash()
    {
        if (!canDash) return;
        canDash = false;
        StartCoroutine(Dash());
    }
    public void OnShoot()
    {

        if (isDashing || weapon == null) return;
        startedShoot = !startedShoot;
        StartCoroutine(ShootRoutine());
    }
    public void OnInteract()
    {
        if (isNearWeapon && weaponNear != null)
        {
            if (weapon == null)
            {
                weaponNear.Equip(this.transform);
                weapon = weaponNear;
                weaponNear = null;
            } 
            else if (weapon != weaponNear) 
            {
                weapon.Desequip();
                weaponNear.Equip(this.transform);
                weapon = weaponNear;
                weapon.LevelUp(2);
                weaponNear = null;
            }
        }
        if (isNearPotion)
        {
            switch (potionNear.type)
            {
                case "Health":
                    this.Heal(potionNear.value);
                    potionNear.Use();
                    break;
                case "Speed":
                    StartCoroutine(IncreaseSpeed(potionNear.value));
                    potionNear.Use();
                    break;
                case "FireRate":
                    if (weapon != null) StartCoroutine(weapon.IncreaseFireRate(potionNear.value));
                    potionNear.Use(); 
                    break;
                case "Gold" or "XP":
                    StartCoroutine(IncreaseDropValue(potionNear.value,potionNear.type));
                    potionNear.Use();
                    break;
                default:
                    break;
            } 
        }
    }

    private Vector2 GetDirection(Vector2 input)
    {
        float X = Mathf.Abs(input.x) > 0.5f ? Mathf.Sign(input.x) : 0;
        float Y = Mathf.Abs(input.y) > 0.5f ? Mathf.Sign(input.y) : 0;

        return new Vector2(X, Y).normalized;
    }

    public void Heal(float heal)
    {
        this.LifePoint = Mathf.Clamp(this.LifePoint+(float) Round(heal*0.01f*MaxLifePoint), 0, MaxLifePoint);
        StartCoroutine(ShowPopup("red", (int) Round(heal * 0.01f * MaxLifePoint)));
    }

    public IEnumerator IncreaseSpeed(float time)
    {
        this.speed *= 2;
        this.dashCooldown /= 2;
        yield return new WaitForSeconds(time);
        this.speed /= 2;
        this.dashCooldown *= 2;
    }

    public IEnumerator IncreaseDropValue(float time,string type)
    {
        if (type == "XP") this.xpValue *= 3; else this.goldValue *= 3;
        yield return new WaitForSeconds(time);
        if (type == "XP") this.xpValue /= 3; else this.goldValue /= 3;
    }
    public void ReceiveDamage(Collider2D collider)
    {
        if (!canBeDamaged) return;
        float damage = 0;
        switch (collider.tag)
        {
            case "Ennemy" or "Boss":
                damage = collider.GetComponent<Ennemy>().GetDamage();
                break;
            case "BossAttack":
                damage = GameObject.FindGameObjectWithTag("Boss").GetComponent<Ennemy>().GetDamage();
                break;
            default:
                break;
        }
        print(damage);
        this.LifePoint = Mathf.Clamp(this.LifePoint - damage, 0, MaxLifePoint*0.7f);
        StartCoroutine(Invulnerabilty());
    }

    private IEnumerator Invulnerabilty()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(1);
        canBeDamaged = true;
    }
    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ennemy"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnnemyAttack"), true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), true);

        isDashing = true;
        rb.linearVelocity = directionDash * dashPower;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.linearVelocity = directionDash * speed * 0.8f;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ennemy"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("EnnemyAttack"), false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    public IEnumerator ShootRoutine()
    {
        while (startedShoot)
        {
            if (weapon != null) {
                weapon.ShootWeapon(directionShoot);
            }
            yield return new WaitForSeconds(weapon.fireRate);
        }
    }

    private void LvlUp()
    {
        if (currentXp >= xpNeeded)
        {
            currentLvl++;
            currentXp = currentXp - xpNeeded;
            xpNeeded = (currentLvl ^ 2) + 100;
            LifePoint += 2;
            MaxLifePoint += 2;
            StartCoroutine(ShowPopup("red", 2));
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Weapon":
                weaponNear = collision.GetComponent<Weapon>();
                isNearWeapon = !weaponNear.isWielded;
                weaponNear.ShowPopup();
                break;
            case "Potion":
                potionNear = collision.GetComponent<Potion>();
                isNearPotion = true;
                potionNear.ShowPopup();
                break;
            case "Xp":
                int value = collision.GetComponent<Xp>().getValue();
                currentXp += value * xpValue;
                StartCoroutine(ShowPopup("blue",value));
                Destroy(collision.gameObject);
                break;
            case "Coin":
                value = collision.GetComponent<Coin>().getValue();
                money += value * goldValue;
                StartCoroutine(ShowPopup("yellow",value));
                Destroy(collision.gameObject);
                break;
            case "BossAttack":
                ReceiveDamage(collision);
                break;
            default:
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            if (weaponNear) weaponNear.ClosePopup();
            weaponNear = null;
            isNearWeapon = false;

        }
        if (collision.CompareTag("Potion"))
        {
            if (potionNear) potionNear.ClosePopup();
            potionNear = null;
            isNearPotion = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ennemy" || collision.gameObject.tag == "Boss")
        {
            ReceiveDamage(collision.gameObject.GetComponent<Collider2D>());
        }
    }

    public IEnumerator ShowPopup(string color, int value)
    {
        popupCanvas.gameObject.SetActive(true);
        textPopUp = GetComponentInChildren<TMP_Text>();
        textPopUp.SetText("+" + value.ToString());
        switch (color)
        {
            case "yellow":
                textPopUp.color = new Color(255, 255, 0);
                break;
            case "blue":
                textPopUp.color = new Color(0, 0, 139);
                break;
            case "red":
                textPopUp.color = new Color(255, 0, 0);
                break;
            default:
                break;
        }
        yield return new WaitForSeconds(1f);
        ClosePopup();
    }

    public void ClosePopup()
    {
        popupCanvas.gameObject.SetActive(false);
    }
    public void OnPause(){
        GameManager.Instance.TogglePause();
    }

    public void OnCrouch() 
    {
        GameManager.Instance.ExitMerchant();
    }

    public void HealthBar()
    {
        var percent = (int) Round(LifePoint * 100 / MaxLifePoint * 0.1);
        print(percent);
        hpBar.sprite = hpSprites[percent];
    }

    public void XpBar()
    {
        var percent = (int)Round(currentXp * 100 / xpNeeded * 0.1);
        xpBar.sprite = xpSprites[percent];
    }
}
