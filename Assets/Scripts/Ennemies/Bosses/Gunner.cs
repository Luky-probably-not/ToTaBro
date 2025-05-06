using UnityEngine;
using System.Collections;

public class Gunner : Ennemy
{
    [SerializeField] protected GameObject bullet;
    [SerializeField] private int bulletSpeed = 6;
    [SerializeField] private int bulletDamage = 2;
    [SerializeField] private int bulletDisparitionTime = 2;
    private bool hasStartedShooting = false;
    [SerializeField] private float preferredDistance = 5f;
    [SerializeField] private float distanceTolerance = 0.5f;
    [SerializeField] private float fireRate = 0.4f;


    protected override void Awake()
    {
        base.Awake();
        hp = 400;
        nickname = "Gunner";
        difficulty = 20;
        damage = 2;
        speed = 4;
    }
    
    void Start()
    {
	InvokeRepeating(nameof(WarningCall), 0f, 7f);
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

    protected override void GoTo(string targetTag)
    {
	FindTarget(targetTag);
	if (target != null)
        {
	    float distance = Vector2.Distance(transform.position, target.position);

	    Vector2 direction = (transform.position - target.position).normalized;

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
	float time = 0.5f;
	SpriteRenderer sr = GetComponent<SpriteRenderer>();
	for(int i = 0; i <= 6; i++)
	{
	    time -= 0.1f;
	    sr.color = Color.red;
	    yield return new WaitForSeconds(time);
	    sr.color = Color.green;
	    yield return new WaitForSeconds(time);
	    sr.color = Color.red;
	}
	rb.linearVelocity = Vector2.zero;
	//BulletRain();
	yield return new WaitForSeconds(0.5f);
	sr.color = Color.green;
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
}
