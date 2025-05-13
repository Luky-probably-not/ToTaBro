using UnityEngine;

public class Speedy : Ennemy
{
    protected override void Awake()
    {
        base.Awake();
        hp = 1;
        nickname = "Speedy";
        difficulty = 2;
        damage = 3;
        speed = 6;
    }
    
    void Start()
    {
        Evoluate(currentWave);
    }

    void Update()
    {
	isDead();
        GoTo("Player");
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
	HandleCollision(collision);
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
	HandleCollision(collision.collider);
    }
}

