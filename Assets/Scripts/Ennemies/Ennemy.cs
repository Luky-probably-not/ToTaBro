using System.Collections;
using UnityEngine;
using System;

public class Ennemy : MonoBehaviour
{
    [Header("Enemy Data")]
    protected float hp;
    protected float hp;
    protected string nickname;
    protected int difficulty;
    protected float damage;
    protected float damage;
    protected int speed;
    
    [Header("Drops")]
    
    [Header("Drops")]
    protected Transform target;
    [SerializeField] protected GameObject xp;
    [SerializeField] protected GameObject coin;

    [Header("Animator")]
	public Animator animator;

    [Header("RigidBody")]

    [Header("Animator")]
	public Animator animator;

    [Header("RigidBody")]
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
            SelectAnimation(direction);
            SelectAnimation(direction);
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
            animator.SetTrigger("IsAttack");
            TakeDamage(1);
        }
        {
            animator.SetTrigger("IsAttack");
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
    }

    protected void TakeDamage(int amount)
    {
        hp -= amount;
    }

    protected bool isDead()
    protected bool isDead()
    {
	    if(hp <= 0)
        {
	    if(hp <= 0)
        {
            Die();
            return true;
        }
        return false;
            return true;
        }
        return false;
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

    public virtual void Evoluate(int wave)
    {
        this.hp *= 1.3f * wave;
        this.damage *= 1.5f * wave;
    }

    protected void SelectAnimation(Vector2 direction)
    {
        direction.Normalize();

        animator.ResetTrigger("LookSide");
        animator.ResetTrigger("LookUp");
        animator.ResetTrigger("LookDown");

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            animator.SetTrigger("LookSide");
            FlipSprite(direction.x < 0);
        }
        else
        {
            if (direction.y > 0)
                animator.SetTrigger("LookUp");
            else
                animator.SetTrigger("LookDown");
        }
    }

    public void FlipSprite(bool facingLeft)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (facingLeft ? -1 : 1);
        transform.localScale = scale;
    }
    
    //Getter
    
    public float GetHp()
    public float GetHp()
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

    public float GetDamage()
    public float GetDamage()
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
