using System.Collections;
using UnityEngine;

public class LaserBullet : Bullet
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
    }

    void Update()
    {
        if (this.player != null)
        {
            transform.position = this.player.position;
        }
    }
}
