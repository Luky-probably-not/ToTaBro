using UnityEngine;
using System.Collections;

public interface IPotion
{
    string type { get; set; }
    float value { get; set; }

    void Use() {}
}