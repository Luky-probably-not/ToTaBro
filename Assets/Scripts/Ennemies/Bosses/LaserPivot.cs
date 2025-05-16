using UnityEngine;

public class LaserPivot : MonoBehaviour
{
    public static float rotationSpeed= 35f;
    void Update()
    {
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }
}
