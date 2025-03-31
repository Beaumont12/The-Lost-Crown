using System;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    public float detectionRange = 2f;
    public float moveSpeed = 1.5f;
    public int maxHealth = 2;
    public float patrolDistance = 4f;
    public bool canPatrol = true; // Toggle this in Inspector

    private int currentHealth;
    private Transform player;
    private Animator animator;
    private Rigidbody2D rb;

    private bool isAttacking = false;
    public bool isDead = false;
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

        if (player.position.x > transform.position.x && !facingRight)
            Flip();
        else if (player.position.x < transform.position.x && facingRight)
            Flip();

        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
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
        }
    }

    public void DealDamage()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= detectionRange + 0.2f)
        {
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage();
        }

        isAttacking = false;
    }

    public void TakeDamage()
    {
        if (isDead) return;

        currentHealth--;

        if (currentHealth <= 0)
        {
            isDead = true;
            animator.SetTrigger("Die");
            rb.linearVelocity = Vector2.zero;
            GetComponent<Collider2D>().enabled = false;
            this.enabled = false;

            // Find the PlayerHealth component and reset player's health
            PlayerHealth playerHealth = UnityEngine.Object.FindFirstObjectByType<PlayerHealth>();  // Find PlayerHealth component
            if (playerHealth != null)
            {
                playerHealth.ResetHealthAfterKill();  // Reset the player's health to 3
            }

            Destroy(gameObject, 1f); // Adjust delay based on Die animation length
        }
        else
        {
            animator.SetTrigger("Hurt");
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
