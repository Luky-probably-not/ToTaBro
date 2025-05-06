using UnityEngine;

public class Ennemy : MonoBehaviour
{
    protected int hp;
    protected string nickname;
    protected int difficulty;
    protected int damage;
    protected int speed;
    protected Transform target;
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
    }

    protected void FindTarget(string targetTag)
    {
        GameObject targetObj = GameObject.FindGameObjectWithTag(targetTag);
        if (targetObj != null)
        {
            target = targetObj.transform;
        }
    }

    protected void GoTo(string targetTag)
    {
        FindTarget(targetTag);
        if (target != null)
        {
	    //rb.bodyType = RigidbodyType2D.Kinematic;
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
	        rb.linearVelocity = Vector2.zero;
	    }
    }

    protected void TakeDamage(int amount)
    {
        print(hp);
        hp -= amount;
        if(hp <= 0)
            Die();
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    //Damge moyen method m*x+p +/-50%
    
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
