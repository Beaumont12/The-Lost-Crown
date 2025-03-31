using System.Diagnostics;
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3; // Max health for the player
    private int currentHealth;

    private HeroKnight heroKnight;
    private Animator animator;

    [Header("Respawn Settings")]
    public float respawnDelay = 2f;
    public Vector3 respawnPosition; // Set this in Inspector if needed

    void Start()
    {
        currentHealth = maxHealth;
        heroKnight = GetComponent<HeroKnight>();
        animator = GetComponent<Animator>();

        // If no custom respawn set, use starting position
        if (respawnPosition == Vector3.zero)
            respawnPosition = transform.position;
    }

    public void TakeDamage()
    {
        if (IsBlocking()) return;

        currentHealth--;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    private bool IsBlocking()
    {
        return animator.GetBool("IdleBlock");
    }

    private void Die()
    {
        animator.SetTrigger("Death");
        heroKnight.enabled = false;

        // Optional: Freeze movement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        // Reset position
        transform.position = respawnPosition;

        // Reset health
        currentHealth = maxHealth;

        // Reset animation state
        animator.ResetTrigger("Death");
        animator.SetTrigger("Idle"); // Optional: ensure it returns to idle

        // Re-enable controls
        heroKnight.enabled = true;
    }

    // Reset player health to 3 after killing an enemy
    public void ResetHealthAfterKill()
    {
        currentHealth = maxHealth;
        UnityEngine.Debug.Log("Player health reset to 3 after enemy death.");
    }
}
