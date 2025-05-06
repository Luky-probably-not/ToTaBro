using System.Collections;
using UnityEngine;

public class XpPotion : Potion
{
    protected override void Awake()
    {
        base.Awake();
        type = "XP";
        value = 5;
        description = "Increase the xp value for xxxxx seconds";
    }
}