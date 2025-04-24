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
    
    void Update()
    {
        GoTo("Player");
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }
}