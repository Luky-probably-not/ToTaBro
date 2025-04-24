using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5;
    private Vector2 directionDash;

    private bool canDash = true;
    private bool isDashing = false;
    private float dashPower = 25f;
    private float dashTime = 0.3f;
    private float dashCooldown = 1f;

    private IWeapon? weapon; 
    private bool startedShoot = false;
    private Vector2 directionShoot;

    private bool isNearWeapon = false;
    private IWeapon? weaponNear;

    private bool isNearPotion = false;
    private IPotion? potionNear;

    public float LifePoint = 5;
    private float MaxLifePoint = 10;

    void Start()
    {
        directionShoot = transform.right;
    }

    void Update()
    {
        if (isDashing) return;

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
        if (isNearWeapon)
        {
            if (weapon == null)
            {
                weaponNear.Equip(this.transform);
                weapon = weaponNear;
            } else {
                weapon.Desequip();
                weaponNear.Equip(this.transform);
                weapon = weaponNear;
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
        this.LifePoint = Mathf.Clamp(this.LifePoint+heal, 0, MaxLifePoint);
    }

    public IEnumerator IncreaseSpeed(float time)
    {
        this.speed *= 2;
        this.dashCooldown /= 2;
        yield return new WaitForSeconds(time);
        this.speed /= 2;
        this.dashCooldown *= 2;
    }

    public void ReceiveDamage(Collider2D collider)
    {
        this.LifePoint = Mathf.Clamp(this.LifePoint - collider.GetComponent<Ennemy>().GetDamage(), 0, MaxLifePoint);
        StartCoroutine(IgnoreCollision(collider, GetComponent<Collider2D>(), 1f));
    }

    private IEnumerator IgnoreCollision(Collider2D a, Collider2D b, float time)
    {
        Physics2D.IgnoreCollision(a, b, true);
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreCollision(a, b, false);
    }
    private IEnumerator Dash()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ennemies"), true);

        isDashing = true;
        rb.linearVelocity = directionDash * dashPower;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        rb.linearVelocity = directionDash * speed * 0.8f;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ennemies"), false);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    
    public IEnumerator ShootRoutine()
    {
        while (startedShoot)
        {
            weapon.ShootWeapon(directionShoot);
            yield return new WaitForSeconds(weapon.fireRate);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            weaponNear = collision.GetComponent<IWeapon>();
            isNearWeapon = !weaponNear.isWielded;
        }
        if (collision.CompareTag("Potion"))
        {
            potionNear = collision.GetComponent<IPotion>();
            isNearPotion = true;
        }
        if (collision.CompareTag("Ennemy"))
        {
            ReceiveDamage(collision);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            weaponNear = null;
            isNearWeapon = false;
        }
        if (collision.CompareTag("Potion"))
        {
            potionNear = null;
            isNearPotion = false;
        }
    }
}
