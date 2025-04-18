using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private float speed = 5;
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

    private int LifePoint = 5;

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
                weapon = weaponNear;
                weapon.player = this.transform;
                weapon.isWielded = true;
            } else
            {
                weapon.player = null;
                weapon.isWielded = false;
                weapon = weaponNear;
                weapon.player = this.transform;
                weapon.isWielded = true;
            }
            print("true");
        } else
        {
            print("false");
        }
    }

    private Vector2 GetDirection(Vector2 input)
    {
        float X = Mathf.Abs(input.x) > 0.5f ? Mathf.Sign(input.x) : 0;
        float Y = Mathf.Abs(input.y) > 0.5f ? Mathf.Sign(input.y) : 0;

        return new Vector2(X, Y).normalized;
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
            weapon.Shoot(directionShoot);
            yield return new WaitForSeconds(weapon.fireRate);
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            weaponNear = collision.GetComponent<IWeapon>();
            isNearWeapon = !weaponNear.isWielded;
            print(weaponNear.name);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            weaponNear = null;
            isNearWeapon = false;
        }
    }
}
