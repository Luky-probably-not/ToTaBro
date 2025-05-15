using UnityEngine;
using System.Collections;

public class Ranged : Ennemy
{
    private float fireRate;
    private float baseFireRate = 3f;
    public GameObject bullet;
    private int bulletSpeed = 6;
    private int bulletDamage = 3;
    private int bulletDisparitionTime = 4;
    private bool hasStartedShooting = false;
    private float preferredDistance = 5f;
    private float distanceTolerance = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        hp = 1;
        nickname = "Ranged";
        difficulty = 3;
        damage = 2;
        speed = 1;
    }

    void Start()
    {
        Evoluate(40);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
	HandleCollision(collision);
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

        FlipSprite(target.position.x < transform.position.x);

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

    protected override void Evoluate(int wave)
    {
        this.hp *= 1.3f * wave;
        this.damage *= 1.5f * wave;
        this.fireRate = baseFireRate / (1f + 0.07f * wave);
    }
}
