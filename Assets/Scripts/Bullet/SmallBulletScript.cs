using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    public float StartedAngle {get; set; }
    public int Damage { get; set; }
    void Start() 
    {
        
    }

    void Update() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Ennemy"))
        {
            StartCoroutine((this as IBullet).IgnoreCollision(collision, GetComponent<Collider2D>(), 1f));
            Destroy(gameObject);
        }
    }

    public void DieAfterSeconds(float time)
    {
        StartCoroutine(Die(time));
    }
    public IEnumerator Die(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
