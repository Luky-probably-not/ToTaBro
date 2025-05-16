using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour
{
    public float StartedAngle;
    public int Damage;
    public bool passThrough;
    public float slash;
    public void Init(bool pass)
    {
        this.passThrough = pass;
    }
    public virtual void Awake() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DeadZone"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(IgnoreCollision(collision, GetComponent<Collider2D>(), 1f));
            if (!passThrough) Destroy(gameObject,0.2f);
        }
    }

    public void DieAfterSeconds(float time)
    {
        StartCoroutine(this.Die(time));
    }

    public IEnumerator Die(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    public IEnumerator IgnoreCollision(Collider2D a, Collider2D b, float time)
    {
        Physics2D.IgnoreCollision(a, b, true);
        yield return new WaitForSeconds(time);
        if (a == null || b == null) yield break;
        Physics2D.IgnoreCollision(a, b, false);
    }
}
