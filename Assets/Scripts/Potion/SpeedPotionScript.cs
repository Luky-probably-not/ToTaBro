using System.Collections;
using UnityEngine;

public class SpeedPotion : Potion
{
    protected override void Awake()
    {
        base.Awake();
        type = "Speed";
        value = 5;
        description = "Increase your movement speed for xxxxx seconds";
    }
}