using System.Collections;
using UnityEngine;

public class HealthPotion : MonoBehaviour, IPotion
{
    public string type { get; set; } = "Health";
    public float value { get; set; } = 3;

    public void Use()
    {
        Destroy(gameObject);
    }
}