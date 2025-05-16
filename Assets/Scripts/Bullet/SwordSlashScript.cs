using System.Collections;
using UnityEngine;

public class SwordSlash : Bullet
{
    Transform player;
    public override void Awake()
    {
        base.Awake();
        passThrough = true;
    }
    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            this.player = playerObj.transform;
        }
        StartedAngle += 42f* slash;
        transform.rotation = Quaternion.Euler(0, 0, StartedAngle);
    }

    void Update()
    {
        if (this.player != null)
        {
            transform.position = this.player.position;
        }
        StartedAngle -= slash;
        transform.rotation = Quaternion.Euler(0, 0, StartedAngle);
    }
}
