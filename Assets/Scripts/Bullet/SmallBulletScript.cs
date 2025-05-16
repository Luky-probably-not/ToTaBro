using System.Collections;
using UnityEngine;

public class SmallBullet : Bullet
{
    public override void Awake()
    {
        base.Awake();
        passThrough = false;
    }
}
