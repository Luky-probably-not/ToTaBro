using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public Vector2 directionDash;

    public bool canDash = true;
    public bool isDashing = false;
    public float dashPower = 25f;
    public float dashTime = 0.3f;
    public float dashCooldown = 1f;

    public GameObject weaponObject;
    //public IWeapon weapon;
    public bool startedShoot = false;
    public Vector2 directionShoot;

    public int LifePoint = 5;

    void Start()
    {
        directionShoot = transform.right;
    }

    void Update()
    {
        /*this.weapon = this.weaponObject.GetComponent<IWeapon>();
        if (weapon != null)
        {
            weapon.player = this.transform;
        }*/
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

    public void OnShoot()
    {

        if (isDashing) return;
        startedShoot = !startedShoot;
        //StartCoroutine(ShootRoutine());
    }
    /*
    public IEnumerator ShootRoutine()
    {
        while (startedShoot)
        {
            weapon.Shoot(directionShoot);
            yield return new WaitForSeconds(weapon.fireRate);
        }
    }*/


    public void OnTriggerEnter2D(Collider2D collision)
    {
        /*Ennemies enemy = collision.GetComponent<Ennemies>();
        if (enemy != null)
        {
            LifePoint = Mathf.Clamp(LifePoint - 1, 0, 5);
            enemy.Deactivate();
        }*/
    }
}
