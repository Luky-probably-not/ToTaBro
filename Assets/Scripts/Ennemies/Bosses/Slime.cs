using UnityEngine;
using System.Collections;

public class Slime : Ennemy
{
	[Header("Attack")]
    [SerializeField] protected GameObject attack;
    protected bool hasSplit = false;
    protected int maxHp = 500;

	private bool canMove = true;
    
    protected override void Awake()
    {
        base.Awake();
        hp = 50;
        nickname = "Slime";
        difficulty = 10;
        damage = 3;
        speed = 1;
    }

    void Start()
    {
		InvokeRepeating(nameof(WarningCall), 5f, 5f);
    }

    void Update()
    {
		isDead();
		if(canMove)
			GoTo("Player");
		Split();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
		HandleCollision(collision);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
		HandleCollision(collision.collider);
    }

    public void WarningCall()
    {
		StartCoroutine(Warning());
    }
    
    public IEnumerator Warning()
    {
		SlamAttack();
		yield return new WaitForSeconds(0.5f);
    }

	public void SlamAttack()
	{
		StartCoroutine(PerformSlamAttack());
	}

	private IEnumerator PerformSlamAttack()
	{
		animator.SetBool("IsAttacking", true);

		yield return new WaitForSeconds(1.3f);

		canMove = false;

		float x = transform.position.x;
		float y = transform.position.y-1f;
		Vector2 pos = new Vector2(x,y);

		GameObject attackInstance = Instantiate(attack, pos, transform.rotation);
		Destroy(attackInstance, 0.5f);

		yield return new WaitForSeconds(0.7f);

		canMove = true;


		animator.SetBool("IsAttacking", false);
	}


    public void Split()
    {
	if(hp <= maxHp/2 && !hasSplit)
	{
	    hasSplit = true;
	    float x1 = transform.position.x+2F;
	    float y1 = transform.position.y;
	    Vector2 pos1 = new Vector2(x1,y1);
	    GameObject baby1 = Instantiate(gameObject, pos1, transform.rotation);
	    
	    float x2 = transform.position.x-2F;
	    float y2 = transform.position.y;
	    Vector2 pos2 = new Vector2(x2,y2);
	    GameObject baby2 = Instantiate(gameObject, pos2, transform.rotation);

	    baby1.GetComponent<Slime>().hasSplit = true;
	    baby2.GetComponent<Slime>().hasSplit = true;
	    Destroy(gameObject);
	}
    }
}
