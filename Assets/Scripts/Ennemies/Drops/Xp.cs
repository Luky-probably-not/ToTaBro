using UnityEngine;

public class Xp : MonoBehaviour
{
    [SerializeField] private int xpValue = 10;

    public int getValue() 
    {
        return xpValue;
    }
}
