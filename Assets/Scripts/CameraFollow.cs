using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Vector3 offset = new Vector3();
    [SerializeField] Transform target = null;

    void Update()
    {
        if (target)
        {
            transform.position = target.position + offset;
        }
    }
}
