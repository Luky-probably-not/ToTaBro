using System.Collections;
using UnityEngine;

public class HealthPotion : Potion
{
    protected override void Awake()
    {
        base.Awake();
        type = "Health";
        value = 3;
        description = "Heal your character for xxxxx health points";
    }
}