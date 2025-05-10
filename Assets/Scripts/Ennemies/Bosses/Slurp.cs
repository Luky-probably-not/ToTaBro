using UnityEngine;
using System.Collections;

public class Slurp : Ennemy
{
    [SerializeField] protected GameObject laser;
    protected int maxHp = 700;
    private int currentPhase = 0;

    [Header("Animator")]
	public Animator animator;

    protected override void Awake()
    {
        base.Awake();
        hp = 700;
        nickname = "Slurp";
        difficulty = 50;
        damage = 5;
        speed = 0;
    }

    void Start()
    {
        SetPhase();
    }

    void Update()
    {
        if(isDead())
            DestroyAllWithName("LaserPivot");

        GoTo("Player");
        SetPhase();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
	    HandleCollision(collision);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
	    HandleCollision(collision.collider);
    }

    void LaserAttackOrbit(Vector3 direction)
    {
        float laserLength = laser.transform.localScale.x;

        GameObject pivot = new GameObject("LaserPivot");
        pivot.transform.position = transform.position;

        float offsetMultiplier = (Mathf.Abs(direction.x) > 0f && Mathf.Abs(direction.y) > 0f) ? 1f : 1f;
        Vector3 offset = direction.normalized * (laserLength / 2f) * offsetMultiplier;

        GameObject laserInstance = Instantiate(laser, pivot.transform.position + offset, Quaternion.identity, pivot.transform);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laserInstance.transform.rotation = Quaternion.Euler(0, 0, angle);

        pivot.AddComponent<LaserPivot>();
    }

    void OrbitLaserInstance()
    {
        LaserAttackOrbit(Vector3.right);
        LaserAttackOrbit(Vector3.left);
        LaserAttackOrbit(Vector3.up);
        LaserAttackOrbit(Vector3.down);
    }

    void OrbitLaserInstanceDiag()
    {
        LaserAttackOrbit((Vector3.up + Vector3.right).normalized);
        LaserAttackOrbit((Vector3.up + Vector3.left).normalized);
        LaserAttackOrbit((Vector3.down + Vector3.right).normalized);
        LaserAttackOrbit((Vector3.down + Vector3.left).normalized);
    }

    void ToggleRotationDirection()
    {
        LaserPivot.rotationSpeed *= -1f;
    }

    void FirstPhase()
    {
        OrbitLaserInstance();
    }

    void SecondPhase()
    {
        OrbitLaserInstance();
        OrbitLaserInstanceDiag();
    }

    void FinalPhase()
    {
        OrbitLaserInstance();
        OrbitLaserInstanceDiag();
        InvokeRepeating(nameof(ToggleRotationDirection), 5f, 5f);
    }

void SetPhase()
{
    int newPhase = 0;
    if (hp > maxHp / 2)
    {
        newPhase = 1; // Première phase
    }
    else if (hp <= maxHp / 2 && hp > maxHp / 4)
    {
        newPhase = 2; // Deuxième phase
    }
    else if (hp <= maxHp / 4)
    {
        newPhase = 3; // Phase finale
    }

    if (newPhase != currentPhase)
    {
        currentPhase = newPhase;
        StartCoroutine(SwitchPhase());
    }
}

    IEnumerator SwitchPhase()
{
    if (currentPhase == 1)
    {
        DestroyAllWithName("LaserPivot");
		animator.SetBool("IsAttacking", true);
		yield return new WaitForSeconds(3.2f);
        FirstPhase();
		animator.SetBool("IsAttacking", false);
    }
    else if (currentPhase == 2)
    {
        DestroyAllWithName("LaserPivot");
        animator.SetBool("IsAttacking", true);
		yield return new WaitForSeconds(3.2f);
        SecondPhase();
		animator.SetBool("IsAttacking", false);
    }
    else if (currentPhase == 3)
    {
        DestroyAllWithName("LaserPivot");
        animator.SetBool("IsAttacking", true);
		yield return new WaitForSeconds(3.2f);
        FinalPhase();
		animator.SetBool("IsAttacking", false);
    }
}

void DestroyAllWithName(string nameToDestroy)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == nameToDestroy)
            {
                Destroy(obj);
            }
        }
    }
}
