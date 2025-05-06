using UnityEngine;
using System.Collections;

public class Slime : Ennemy
{
    [SerializeField] protected GameObject attack;
    protected bool hasSplit = false;
    protected int maxHp = 500;
    
    protected override void Awake()
    {
        base.Awake();
        hp = 100;
        nickname = "Boss";
        difficulty = 10;
        damage = 3;
        speed = 1;
    }

    void Start()
    {
	InvokeRepeating(nameof(WarningCall), 0f, 7f);
    }

    void Update()
    {
	GoTo("Player");
	Split();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
	HandleCollision(collision);
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
	SlamAttack();
	yield return new WaitForSeconds(0.5f);
	sr.color = Color.green;
    }
    
    public void SlamAttack()
    {
	GameObject attackInstance = Instantiate(attack, transform.position, transform.rotation);
	Destroy(attackInstance, 0.5f);
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
