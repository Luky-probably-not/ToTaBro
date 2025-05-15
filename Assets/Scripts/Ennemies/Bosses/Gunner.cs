using UnityEngine;
using System.Collections;

public class Gunner : Ennemy
{
    [Header("Bullet Object")]
    [SerializeField] protected GameObject bullet;
    
    [Header("Bullet Data")]
    private int bulletSpeed = 6;
    private int bulletDamage = 2;
    private int bulletDisparitionTime = 2;
    private bool hasStartedShooting = false;
    private float fireRate = 0.4f;

    [Header("Disante Data")]
    private float preferredDistance = 5f;
    private float distanceTolerance = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        hp = 400;
        nickname = "Gunner";
        difficulty = 20;
        damage = 2f;
        speed = 4;
    }
    
    void Start()
    {
	    InvokeRepeating(nameof(WarningCall), 7f, 7f);
    }

    void Update()
    {
        isDead();
        GoTo("Player");
        if (!hasStartedShooting)
        {
            StartCoroutine(ShootRoutine());
            hasStartedShooting = !hasStartedShooting;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
	    HandleCollision(collider);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
	    HandleCollision(collision.collider);
    }

    protected override void GoTo(string targetTag)
    {
	FindTarget(targetTag);
	if (target != null)
        {
	    float distance = Vector2.Distance(transform.position, target.position);

	    Vector2 direction = (transform.position - target.position).normalized;

        FlipSprite(target.position.x > transform.position.x);

	    if (distance < preferredDistance - distanceTolerance)
	    {
		    transform.position += (Vector3)direction * speed * Time.deltaTime;
	    }
	    else if (distance > preferredDistance + distanceTolerance)
	    {
            direction = -direction;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
	    }
        }
    }

    public void WarningCall()
    {
	    StartCoroutine(Warning());
    }
    public IEnumerator Warning()
    {
        animator.SetBool("IsAttacking", true);
		yield return new WaitForSeconds(1.12f);
        Vector2 direction = (target.position - transform.position).normalized;
        Barrage(direction, -40, 5F);
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator ShootRoutine()
    {
        while (true)
        {
	    Vector2 direction = (target.position - transform.position).normalized;
	    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
	    Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
	    Shoot(direction, angle, rotation);
            yield return new WaitForSeconds(this.fireRate);
        }
    }

    public EnemyBullet Shoot(Vector2 directionShoot, float angle, Quaternion rotateDirection)
    {        
        GameObject newBullet = Instantiate(bullet);
        newBullet.transform.SetPositionAndRotation(transform.position, rotateDirection);
        Rigidbody2D newBulletRb = newBullet.GetComponentInChildren<Rigidbody2D>();
        newBulletRb.linearVelocity = directionShoot * this.bulletSpeed;
        newBullet.transform.rotation = Quaternion.Euler(0, 0, angle);
        EnemyBullet bulletObject = newBullet.GetComponent<EnemyBullet>();
        bulletObject.StartedAngle = angle;
        bulletObject.DieAfterSeconds(this.bulletDisparitionTime);
        bulletObject.Damage = bulletDamage;
        return bulletObject;
    }

    public IEnumerator PerformBarrage(Vector2 directionShoot, float startAngle, float angleStep)
    {
        for (float i = 0; i < 16; i++)
        {
            float angleOff = startAngle + i * angleStep;
	    
            Vector2 rotateDirection = Quaternion.Euler(0, 0, angleOff) * directionShoot.normalized;

	    float fAngle = Mathf.Atan2(rotateDirection.y, rotateDirection.x) * Mathf.Rad2Deg;

	    Quaternion rotation = Quaternion.Euler(0, 0, fAngle);

            Shoot(rotateDirection.normalized, fAngle, rotation);
        }
		yield return new WaitForSeconds(0.8f);

		animator.SetBool("IsAttacking", false);
    }

    void Barrage(Vector2 directionShoot, float startAngle, float angleStep)
    {
        StartCoroutine(PerformBarrage(directionShoot, startAngle, angleStep));
    }
}
