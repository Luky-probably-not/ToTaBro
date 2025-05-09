using UnityEngine;

public class Xp : MonoBehaviour
{
    [SerializeField] private int xpValue = 10;

    public int GetXp() { return xpValue; }
}
