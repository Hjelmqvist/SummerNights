using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance { get; protected set; }

    [SerializeField] float movementSpeed = 1;

    [Header("Attacks")]
    [Space(20)]
    [SerializeField] PlayerSpray spray = null;

    public bool CanAttack { get; set; } = true;
    public Animator Animator { get; protected set; }
    public Rigidbody RB { get; protected set; }

    void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        Instance = this;
        Animator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        spray.Use(this);
    }

    void FixedUpdate()
    {
        Move();

        void Move()
        {
            if (IsStunned)
                return;

            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 movement = new Vector3(horizontal, 0, vertical);

            Animator.SetFloat("Horizontal", movement.x);
            Animator.SetFloat("Vertical", movement.z);
            Animator.SetFloat("Magnitude", movement.magnitude);

            RB.velocity = Vector3.ClampMagnitude(movement, 1) * movementSpeed;
        }
    }

    [Header("Stun")]
    [Space(20)]
    [SerializeField] float stunTime = 1;
    [SerializeField] float immuneTime = 3;
    Coroutine stun = null;

    public bool IsStunned { get; protected set; }

    public void Stun()
    {
        if (stun != null)
            return;

        stun = StartCoroutine(StunCoroutine());

        IEnumerator StunCoroutine()
        {
            IsStunned = true;
            RigidbodyConstraints normalConst = RB.constraints;
            RB.constraints = RigidbodyConstraints.FreezeAll;

            float doneTime = Time.time + stunTime;
            while (Time.time < doneTime)
            {
                Vector3 randomDir = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));

                Animator.SetFloat("Horizontal", randomDir.x);
                Animator.SetFloat("Vertical", randomDir.z);
                Animator.SetFloat("Magnitude", randomDir.magnitude);
                yield return null;
            }

            IsStunned = false;
            RB.constraints = normalConst;
            yield return new WaitForSeconds(immuneTime);
            stun = null;
        }
    }
}
