using UnityEngine;

public class Bot : Ennemy
{
    protected override void Awake()
    {
        base.Awake();
        hp = 3;
        nickname = "Bot";
        difficulty = 1;
        damage = 1;
        speed = 3;
    }
    
    void Update()
    {
        GoTo("Player");
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }
}

