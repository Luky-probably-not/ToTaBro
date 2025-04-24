using UnityEngine;

public class Bot : Ennemy
{
    protected override void Awake()
    {
        base.Awake();
        hp = 1;
        nickname = "Bot";
        difficulty = 1;
        damage = 1;
        speed = 3;
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

