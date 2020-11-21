using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerSpray
{
    [SerializeField] SprayProjectile spray = null;
    [SerializeField] float sprayUsesPerSecond = 1;
    [SerializeField] float distanceFromMiddle = 0.5f;
    [SerializeField] float movementMultiplier = 0.5f;

    public void Use(PlayerMovement player)
    {
        if (player.CanAttack && !player.IsStunned)
            player.StartCoroutine(Spray());

        IEnumerator Spray()
        {
            float horizontal = Input.GetAxisRaw("Spray Horizontal");
            float vertical = Input.GetAxisRaw("Spray Vertical");

            player.Animator.SetBool("IsShooting", horizontal != 0 || vertical != 0);

            if (horizontal != 0 || vertical != 0)
            {
                player.CanAttack = false;
                
                Vector3 direction = new Vector3();
                if (horizontal != 0)
                    direction = new Vector3(horizontal, 0, 0);
                else
                    direction = new Vector3(0, 0, vertical);

                player.Animator.SetFloat("Spray Horizontal", direction.x);
                player.Animator.SetFloat("Spray Vertical", direction.z);
                player.Animator.SetFloat("Magnitude", direction.magnitude);

                SprayProjectile s = Object.Instantiate(spray, player.transform.position + direction * distanceFromMiddle, spray.transform.rotation);

                if (direction.x != 0)
                    direction.z = player.Animator.GetFloat("Vertical") * movementMultiplier;
                else
                    direction.x = player.Animator.GetFloat("Horizontal") * movementMultiplier;

                s.Fire(direction);

                yield return new WaitForSeconds(1 / sprayUsesPerSecond);
                player.CanAttack = true;
            }
        }
    }
}
