using System.Collections;
using UnityEngine;

public class LaserBullet : MonoBehaviour, IBullet
{
    public Transform player;
    public float StartedAngle { get; set; }
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            this.player = playerObj.transform;
        }
    }

    void Update()
    {
        if (this.player != null)
        {
            transform.position = this.player.position;
        }
    }

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
