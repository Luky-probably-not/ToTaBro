using System.Collections;
using UnityEngine;

public class SpeedPotion : MonoBehaviour, IPotion
{
    public string type { get; set; } = "Speed";
    public float value { get; set; } = 5;

    public void Use()
    {
        Destroy(gameObject);
    }
}