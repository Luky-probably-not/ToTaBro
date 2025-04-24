using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour, IBullet
{
    public float StartedAngle {get; set; }
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
