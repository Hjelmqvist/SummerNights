using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    [SerializeField] protected float movementSpeed = 5;
    protected Transform target = null;

    protected Vector3 spawnpos = new Vector3();
    protected bool isGoingBack = false;
    protected bool isDead = false;

    public Animator Animator { get; protected set; }
    public AudioSource Audio { get; protected set; }
    public Collider Collider { get; protected set; }
    public Rigidbody RB { get; protected set; }

    public delegate void Death(Enemy enemy);
    public event Death OnDeath;

    protected virtual void Awake()
    {
        Animator = GetComponent<Animator>();
        Audio = GetComponent<AudioSource>();
        Collider = GetComponent<Collider>();
        RB = GetComponent<Rigidbody>();
        spawnpos = transform.position;
    }

    protected virtual void FixedUpdate()
    {
        if (isDead)
            return;

        Vector3 targetPos = spawnpos;
        if (!isGoingBack && target)
            targetPos = target.position;

        Vector3 dir = targetPos - transform.position;
        dir.y = transform.position.y;
        dir = Vector3.ClampMagnitude(dir, 1);

        Animator.SetFloat("Horizontal", dir.x);
        Animator.SetFloat("Vertical", dir.z);
        RB.velocity = Vector3.zero;
        RB.MovePosition(transform.position + dir * movementSpeed * Time.deltaTime);

        if (isGoingBack && Vector3.Distance(transform.position, spawnpos) < 0.1f)
        {
            PlayerHealth.Instance.TakeDamage();
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public virtual bool ModifyHealth(int amount)
    {
        if (amount < 0 && Audio)
            Audio.Play();

        health += amount;
        if (health <= 0)
            Die();
        return health <= 0;

        void Die()
        {
            OnDeath?.Invoke(this);
            Collider.enabled = false;
            RB.isKinematic = true;
            isDead = true;
            target = null;
            if (Animator.GetFloat("Horizontal") > 0)
            {
                Vector3 curScale = transform.localScale;
                curScale.x *= -1;
                transform.localScale = curScale;
            }
            Animator.SetTrigger("Die");
        } 
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void ModifyMovementSpeed(float speed)
    {
        movementSpeed = speed;
    }

    public virtual bool SetTarget(Transform transform)
    {
        if (transform == null)
            return false;

        target = transform;
        return true;
    }

    protected virtual void OnTriggerEnter(Collider col)
    {
        if (col.transform == target)
            isGoingBack = true;
    }
}
