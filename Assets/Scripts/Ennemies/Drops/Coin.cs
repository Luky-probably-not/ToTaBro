using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int coinValue = 1;

    public int getValue() 
    {
        return coinValue;
    }
}
