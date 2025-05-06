using UnityEngine;
using System;

public class Ennemy : MonoBehaviour
{
    protected int hp;
    protected string nickname;
    protected int difficulty;
    protected int damage;
    protected int speed;
    protected Transform target;
    [SerializeField] protected GameObject xp;
    [SerializeField] protected GameObject coin;
    [SerializeField] protected Rigidbody2D rb;

    public void Init(int hp, string nickname, int difficulty, int damage, int speed)
    {
        this.hp = hp;
        this.nickname = nickname;
        this.difficulty = difficulty;
        this.damage = damage;
        this.speed = speed;
    }
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
	rb.mass = 9999f;
    }

    protected void FindTarget(string targetTag)
    {
        GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
    }

    protected virtual void GoTo(string targetTag)
    {
        FindTarget(targetTag);
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;
        }
    }

    protected void HandleCollision(Collider2D collision)
    {
        if(collision.CompareTag("SmallBulletPlayer") || collision.CompareTag("LaserBulletPlayer") || collision.CompareTag("SwordSlashPlayer"))
        {
            Bullet bullet = collision.GetComponentInChildren<Bullet>();
            TakeDamage(bullet.Damage); // Value of damage taken
        }else if (collision.CompareTag("Player"))
	{
	    TakeDamage(1);
	}
    }
    
    protected void DropXp(float posX)
    {
	float x = transform.position.x-posX;
	float y = transform.position.y;
	Vector2 pos = new Vector2(x,y);	
	Instantiate(xp, pos, transform.rotation);
    }

    protected void DropCoin()
    {
	float x = transform.position.x+0.4F;
	float y = transform.position.y;
	Vector2 pos = new Vector2(x,y);
	Instantiate(coin, pos, transform.rotation);
    }

    protected bool isCoinDroped()
    {
    	System.Random rnd = new System.Random();
	int rndNumber = rnd.Next(0,100);
	if(rndNumber >= 40)
	{
	    return true;
	}
	return false;
	    {
	        TakeDamage(1);
	        rb.linearVelocity = Vector2.zero;
	    }
    }

    protected void TakeDamage(int amount)
    {
        print(hp);
        hp -= amount;
    }

    protected void isDead()
    {
	if(hp <= 0)
            Die();
    }

    protected virtual void Die()
        {
        float posX = 0;
        if(isCoinDroped())
        {
            posX = 0.4F;
            DropCoin();
        }
        DropXp(posX);
        GameManager.Instance.EnemyDefeated(1f);
        Destroy(gameObject);
    }
    
    //Getter
    
    public int GetHp()
    {
	return this.hp;
    }

    public string GetNickname()
    {
	return this.nickname;
    }

    public int GetDifficulty()
    {
	return this.difficulty;
    }

    public int GetDamage()
    {
	return this.damage;
    }

    public int GetSpeed()
    {
	return this.speed;
    }

    //Setter

    public void SetHp(int hp)
    {
	this.hp = hp;
    }

    public void SetNickname(string nickname)
    {
	this.nickname = nickname;
    }

    public void SetDifficulty(int difficulty)
    {
	this.difficulty = difficulty;
    }

    public void SetDamage(int damage)
    {
	this.damage = damage;
    }

    public void SetSpeed(int speed)
    {
	this.speed = speed;
    }
}
