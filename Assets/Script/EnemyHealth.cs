using NUnit.Framework;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 100;

    private int currentHealth;
    private bool isDead = false;

    [Header("Components")]
    public EnemyChase EnemyChase;
    public Rigidbody2D rb;
    public Collider2D enemyCollider;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (EnemyChase != null)
        {
            EnemyChase.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        Destroy(gameObject);
    }
    
}
