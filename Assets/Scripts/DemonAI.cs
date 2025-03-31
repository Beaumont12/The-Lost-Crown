using System;
using UnityEngine;

public class DemonAI : MonoBehaviour
{
    public float detectionRange = 2f;
    public float moveSpeed = 1.5f;
    public int maxHealth = 3; // Demon has 3 hitpoints
    public float patrolDistance = 4f;
    public bool canPatrol = true;

    private int currentHealth;
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isAttacking = false;
    private bool isDead = false;
    private bool facingRight = false;

    private Vector2 startPoint;
    private Vector2 patrolTarget;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startPoint = transform.position;
        patrolTarget = startPoint + Vector2.right * patrolDistance;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        Debug.Log("Demon initialized with " + currentHealth + " health.");
    }

    void Update()
    {
        if (isDead)
            return;

        float distanceToPlayer = player != null ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;

        if (distanceToPlayer <= detectionRange && player != null)
        {
            HandlePlayerChase();
        }
        else
        {
            if (canPatrol)
                Patrol();
            else
                animator.SetBool("isMoving", false); // Stay idle when not patrolling
        }
    }

    void HandlePlayerChase()
    {
        animator.SetBool("isMoving", false);
        Debug.Log("Chasing player at position: " + player.position);

        if (player.position.x > transform.position.x && !facingRight)
            Flip();
        else if (player.position.x < transform.position.x && facingRight)
            Flip();

        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");  // Trigger the Attack animation
            Debug.Log("Attacking the player.");
        }
    }

    void Patrol()
    {
        animator.SetBool("isMoving", true);

        Vector2 target = facingRight ? patrolTarget : startPoint;
        Vector2 newPos = Vector2.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), moveSpeed * Time.deltaTime);
        rb.MovePosition(newPos);

        if (Mathf.Abs(transform.position.x - target.x) < 0.1f)
        {
            Flip();
            Debug.Log("Patrol reached target, flipping.");
        }
    }

    // Call this method using an animation event when the attack animation hits
    public void DealDamage()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= detectionRange + 0.2f)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage();
                Debug.Log("Dealing damage to the player.");
            }
        }

        isAttacking = false; // Ready for the next attack
    }

    // Call this when the Demon takes damage
    public void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;
        UnityEngine.Debug.Log("Demon health: " + currentHealth); // Add this log to track health

        if (currentHealth <= 0)
        {
            isDead = true;
            UnityEngine.Debug.Log("Demon died."); // Debug log for death
            animator.SetTrigger("Die"); // Trigger the die animation
            rb.linearVelocity = Vector2.zero; // Make sure velocity is zero to stop movement
            GetComponent<Collider2D>().enabled = false; // Disable collider to prevent interaction

            // Optional: Disable other scripts if necessary to prevent further interaction
            this.enabled = false;

            // Destroy the demon after the death animation plays
            Destroy(gameObject, 1f); // Adjust delay based on animation length
        }
        else
        {
            animator.SetTrigger("Hurt");
            UnityEngine.Debug.Log("Demon hurt, triggering hurt animation.");
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        Debug.Log("Demon flipped. Facing right: " + facingRight);
    }
}
