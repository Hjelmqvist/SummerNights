using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    [SerializeField] float secondsBeforeStart = 10;
    [SerializeField] GameObject victoryScreen = null;
    [SerializeField] float victoryTimeScale = 0.5f;
    [SerializeField] float victoryTime = 5;

    [Space(20)]
    [SerializeField] Transform target = null;
    [SerializeField] Transform[] spawnpoints = null;
    [SerializeField] EnemyInfo[] enemies = null;
    List<Enemy> aliveEnemies = new List<Enemy>();

    [System.Serializable]
    class EnemyInfo
    {
        public Enemy enemy = null;
        public int number = 0;
        public float movementSpeed = 5;
        public float timeUntilNextEnemy = 0.5f;
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(secondsBeforeStart);

        foreach (EnemyInfo enemy in enemies)
        {
            for (int i = 0; i < enemy.number; i++)
            {
                Transform spawnpoint = spawnpoints[Random.Range(0, spawnpoints.Length)];
                Enemy e = Instantiate(enemy.enemy, spawnpoint.position, enemy.enemy.transform.rotation);
                e.ModifyMovementSpeed(enemy.movementSpeed);
                e.SetTarget(target);
                e.OnDeath += E_OnDeath;
                aliveEnemies.Add(e);
                yield return new WaitForSeconds(enemy.timeUntilNextEnemy);
            }
        }

        yield return new WaitUntil(() => aliveEnemies.Count == 0);
        Time.timeScale = victoryTimeScale;
        victoryScreen.SetActive(true);

        PlayerMovement.Instance.enabled = false;
        PlayerMovement.Instance.RB.velocity = Vector3.zero;
        PlayerMovement.Instance.Animator.SetFloat("Magnitude", 0);
        PlayerMovement.Instance.Animator.SetBool("IsShooting", false);

        yield return new WaitForSeconds(victoryTime * victoryTimeScale);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void E_OnDeath(Enemy enemy)
    {
        aliveEnemies.Remove(enemy);
    }
}
