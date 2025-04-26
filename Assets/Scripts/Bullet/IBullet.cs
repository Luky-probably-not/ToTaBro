using UnityEngine;
using System.Collections;

public interface IBullet
{
    float StartedAngle { get; set; }

    int Damage { get; set; }
    void DieAfterSeconds(float time) { }

    public IEnumerator IgnoreCollision(Collider2D a, Collider2D b, float time)
    {
        Physics2D.IgnoreCollision(a, b, true);
        yield return new WaitForSeconds(time);
        if (a == null || b == null) yield break;
        Physics2D.IgnoreCollision(a, b, false);
    }
}