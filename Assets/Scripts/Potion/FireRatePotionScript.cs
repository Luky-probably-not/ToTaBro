using System.Collections;
using UnityEngine;

public class FireRatePotion : MonoBehaviour, IPotion
{
    public string type { get; set; } = "FireRate";
    public float value { get; set; } = 5;

    public void Use()
    {
        Destroy(gameObject);
    }
}