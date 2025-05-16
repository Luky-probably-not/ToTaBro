using UnityEngine;

public class Tank : Ennemy
{
    protected override void Awake()
    {
        base.Awake();
        hp = 10;
        nickname = "Tank";
        difficulty = 2;
        damage = 1;
        speed = 1;
    }
    
<<<<<<< HEAD
=======
    void Start()
    {
        //Evoluate(currentWave);
    }

>>>>>>> origin/feat/Player
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
