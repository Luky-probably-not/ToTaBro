using UnityEngine;
using System.Collections;

public interface IBullet
{
    float StartedAngle { get; set; }
    void DieAfterSeconds(float time) { }
}