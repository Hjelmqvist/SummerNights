using UnityEngine;

public class Mosquito : Enemy
{
    [SerializeField] float stunRange = 1;
    protected override void Awake()
    {
        base.Awake();
        target = PlayerMovement.Instance.transform;
    }

    protected override void FixedUpdate()
    {
        if (isDead)
            return;

        if (target)
        {
            Vector3 targetPos = target.position;
            if (Vector3.Distance(transform.position, targetPos) < stunRange)
                PlayerMovement.Instance.Stun();
            else
            {
                Vector3 dir = new Vector3();
                dir = targetPos - transform.position;
                dir.y = transform.position.y;
                dir = Vector3.ClampMagnitude(dir, 1);

                Animator.SetFloat("Horizontal", dir.x);
                Animator.SetFloat("Vertical", dir.z);
                RB.velocity = Vector3.zero;
                RB.MovePosition(transform.position + dir * movementSpeed * Time.deltaTime);
            } 
        }
    }

    public override bool SetTarget(Transform transform)
    {
        //Do not change target;
        return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, stunRange);
    }
}
