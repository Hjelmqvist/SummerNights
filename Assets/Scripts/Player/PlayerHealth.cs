using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// HOTDOGS
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; protected set; }

    [SerializeField] GameObject[] hotdogs = null;
    [SerializeField] GameObject gameover = null;
    [SerializeField] float gameoverTimeScale = 0.5f;
    [SerializeField] float gameoverTime = 5;
    int health = 5;

    void Awake()
    {
        if (Instance)
            Destroy(gameObject);
        Instance = this;
        Time.timeScale = 1;
        health = hotdogs.Length;
    }

    public void TakeDamage()
    {
        health--;
        for (int i = 0; i < hotdogs.Length; i++)
        {
            if (i < health)
                hotdogs[i].SetActive(true);
            else
                hotdogs[i].SetActive(false);
        }

        if (health <= 0)
            StartCoroutine(GameOver());

        IEnumerator GameOver()
        {
            Time.timeScale = gameoverTimeScale;
            gameover.SetActive(true);

            PlayerMovement.Instance.enabled = false;
            PlayerMovement.Instance.RB.velocity = Vector3.zero;
            PlayerMovement.Instance.Animator.SetFloat("Magnitude", 0);
            PlayerMovement.Instance.Animator.SetBool("IsShooting", false);

            yield return new WaitForSeconds(gameoverTime * gameoverTimeScale);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
