using UnityEngine;

public class Ranged : Ennemy
{
    [SerializeField] float fireRate;
    public GameObject bullet;
    public float lastShotTime;

    protected override void Awake()
    {
        base.Awake();
        hp = 1;
        nickname = "Ranged";
        difficulty = 3;
        damage = 2;
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
    // TODO add ranged attack
}