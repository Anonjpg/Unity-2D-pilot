using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health")]
    public int maxHealth = 100;

    [Header("Finisher")]
    public int finisherDamage = 35;
    public float knockbackForce = 8f;


    [Header("Knockdown Setting")]
    public int knockdownThreshold = 40;
    public float knockdownDuration = 5f;

    private int accumulatedDamage = 0;
    public bool IsKnockedDown {get; private set;}
    private Coroutine knockdownCoroutine;

    private int currentHealth;
    private bool isDead = false;

    [Header("Components")]
    public EnemyChase EnemyChase;
    public Rigidbody2D rb;
    public Collider2D enemyCollider;
    public EnemyFlash enemyFlash;
    public EnemyHealthBar healthBar;
    private RigidbodyConstraints2D originalConstraints;

    public PromptFade UIhit;

    void Start()
    {
        currentHealth = maxHealth;

        if (rb != null)
        {
            originalConstraints = rb.constraints;
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= damage;

        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        accumulatedDamage += damage;


        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        
        if (enemyFlash != null)
        {
            enemyFlash.Flash();
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        if (accumulatedDamage >= knockdownThreshold)
        {
            KnockDown();
        }
    }

    void KnockDown()
    {
        if (UIhit != null)
        {
            UIhit.Show();
        }

        IsKnockedDown = true;
        accumulatedDamage = 0;

        if (EnemyChase != null)
        {
            EnemyChase.enabled = false;
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        knockdownCoroutine = StartCoroutine(knockdownTimer());
    }

    IEnumerator knockdownTimer()
    {
        yield return new WaitForSeconds(knockdownDuration);

        if (isDead)
        {
            yield break;
        }

        IsKnockedDown = false;

        if (UIhit != null)
        {
            UIhit.Hide();
        }

        if (rb != null)
        {
            rb.constraints = originalConstraints;
        }

        if (EnemyChase != null)
        {
            EnemyChase.enabled = true;
        }

        knockdownCoroutine = null;
    }

    public void PerformFinisher()
    {
        if (!IsKnockedDown || isDead)
        {
            return;
        }

        if (knockdownCoroutine != null)
        {
            StopCoroutine(knockdownCoroutine);
            knockdownCoroutine = null;
        }

        if (UIhit != null)
        {
            UIhit.Hide();
        }
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        IsKnockedDown = false;

        if (EnemyChase != null)
        {
            EnemyChase.enabled = false;
        }


        StartCoroutine(DieDelay());

        UIhit.Hide();
    }

    public void ApplyFinisherImpact(Vector2 direction)
    {
        if (isDead) return;

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
        }

        TakeDamage(finisherDamage);

        IsKnockedDown = false;

        if (UIhit != null)
        {
            UIhit.Hide();
        }

    }

    IEnumerator DieDelay()
    {
        yield return new WaitForSeconds(0.8f);

        Destroy(gameObject);
    }
    
}
