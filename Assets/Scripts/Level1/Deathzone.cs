using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DeathZone : MonoBehaviour
{
    private Animator playerAnimator;
    private bool isDead = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isDead)
        {
            isDead = true;
            playerAnimator = collision.GetComponent<Animator>();

            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("Death"); // Triggers the death animation
            }

            collision.GetComponent<HeroKnight>().enabled = false; // Disable movement
            collision.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; // Stop movement

            StartCoroutine(RestartLevelAfterDelay(2f)); // Restart after 2 seconds
        }
    }

    IEnumerator RestartLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
