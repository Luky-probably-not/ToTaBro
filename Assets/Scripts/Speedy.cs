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
    
    void Update()
    {
        GoTo("Player");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }
}

