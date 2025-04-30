using UnityEngine;

public class ClampToScreen : MonoBehaviour
{
    private Camera cam;
    private float zOffset = 0f;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        pos.z = zOffset;

        transform.position = pos;
    }
}
