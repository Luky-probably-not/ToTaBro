using System.Collections;
using UnityEngine;

public class SwordSlash : MonoBehaviour, IBullet
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
        StartedAngle += 42f;
        transform.rotation = Quaternion.Euler(0, 0, StartedAngle);
    }

    void Update()
    {
        if (this.player != null)
        {
            transform.position = this.player.position;
        }
        StartedAngle -= 1f;
        transform.rotation = Quaternion.Euler(0, 0, StartedAngle);
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
        print(StartedAngle);
        Destroy(gameObject);
    }
}
