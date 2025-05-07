using System.Collections;
using UnityEngine;

public class HealthPotion : Potion
{
    protected override void Awake()
    {
        base.Awake();
        type = "Health";
        value = 60;
        description = "Heal your character for xxxxx% health points";
    }
}