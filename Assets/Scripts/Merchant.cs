using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Merchant : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.EnterMerchant(this);
    }
}
