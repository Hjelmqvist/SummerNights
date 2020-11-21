using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SprayProjectile : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5;
    [SerializeField] float timeAlive = 1;
    [SerializeField] int damage = 50;

    Rigidbody rb = null;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Invoke("Die", timeAlive);
    }

    void Die()
    {
        Destroy(gameObject);
    }

    public void Fire(Vector3 direction)
    {
        rb.velocity = direction * movementSpeed;
    }

    void OnTriggerEnter(Collider col)
    {
        Enemy e = col.GetComponent<Enemy>();
        if (e)
            e.ModifyHealth(-damage);
    }
}
