using System.Collections;
using UnityEngine;

public class FireRatePotion : Potion
{
    protected override void Awake()
    {
        base.Awake();
        type = "FireRate";
        value = 50;
        description = "Increase the fire rate of your weapon during xxxxx seconds";
    }
}