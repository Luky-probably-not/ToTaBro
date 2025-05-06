using System.Collections;
using UnityEngine;

public class GoldPotion : Potion
{
    protected override void Awake()
    {
        base.Awake();
        type = "Gold";
        value = 5;
        description = "Increase the coin value for xxxxx seconds";
    }
}