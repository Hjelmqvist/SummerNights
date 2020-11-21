using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class LayerUpdater : MonoBehaviour
{
    [SerializeField] int layerOffset = 0;
    [SerializeField] bool onlyUpdateOnStart = false;
    SpriteRenderer sr = null;

    void Awake()
    { 
        sr = GetComponent<SpriteRenderer>();
        if (onlyUpdateOnStart)
        {
            sr.sortingOrder = -(int)(transform.position.z * 10) + layerOffset;
            Destroy(this);
        }
    }

    void LateUpdate()
    {
        sr.sortingOrder = -(int)(transform.position.z * 10) + layerOffset;
    }
}
