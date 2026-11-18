using System;
using System.Collections;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEditor.Search;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
using Unity.Collections;
using System.Threading;

public class PlayerFinisher : MonoBehaviour
{
    [Header("Finisher")]
    public float finisherRange = 4f;
    public float approachDistance = 0.8f;
    public float approachSpeed = 4f;
    public float finisherAnimationDuration = 1.2f;
    public KeyCode finisherKey = KeyCode.E;

    [Header("refrences")]
    public PlayerMovement playerMovement;
    public Rigidbody2D playerRb;
    public Animator animator;
    public CinemachineImpulseSource impulseSource;
    public EnemyHealth enemyTarget;
    public AudioClip sfx;
    public AudioSource Finishing;

    private bool isFinishing = false;
    private bool hasTriggeredImpact = false;

    void Update()
    {
        if (isFinishing)
        {
            return;
        }

        if (Input.GetKeyDown(finisherKey))
        {
            TryFinisher();
        }
    }

    void TryFinisher()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, finisherRange);

        EnemyHealth targetEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();

            if (enemy != null && enemy.IsKnockedDown)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetEnemy = enemy;
                }
            }
        }

        if (targetEnemy != null)
        {
            StartCoroutine(FinisherSequence(targetEnemy));
        }
    }

    IEnumerator FinisherSequence(EnemyHealth enemy)
    {
        isFinishing = true;
        playerMovement.isFinishing = true;
        hasTriggeredImpact = false;

        playerRb.linearVelocity = Vector2.zero;

        bool faceRight = enemy.transform.position.x > transform.position.x;
        playerMovement.FaceDirection(faceRight);

        Vector2 direction = (enemy.transform.position - transform.position).normalized;

        Vector2 targetPosition = (Vector2)enemy.transform.position - direction * approachDistance;

        // Tentukan posisi berhenti di depan enemy
        float targetX = enemy.transform.position.x
                        + (faceRight ? -approachDistance : approachDistance);

        // Player bergerak otomatis mendekati enemy
        while (Mathf.Abs(playerRb.position.x - targetX) > 0.05f)
        {
            float remaining = targetX - playerRb.position.x;

            float speed = Mathf.Min(
                approachSpeed,
                Mathf.Abs(remaining) / Time.fixedDeltaTime
            );

            float movementX = Mathf.Sign(remaining) * speed;

            playerRb.linearVelocity = new Vector2(
                movementX,
                playerRb.linearVelocity.y
            );

            animator.SetTrigger("isRunFinish");

            yield return new WaitForFixedUpdate();
        }

        // Hentikan gerakan horizontal
        playerRb.linearVelocity = new Vector2(
            0f,
            playerRb.linearVelocity.y
        );

        playerRb.linearVelocity = Vector2.zero;

        animator.SetFloat("xVelocity", 0);
        animator.SetFloat("yVelocity", 0f);

        animator.SetTrigger("Finisher");
        impulseSource.GenerateImpulse();
        Thread.Sleep(50);
        KnockBack();
        Finishing.PlayOneShot(sfx);

        yield return new WaitForSeconds(finisherAnimationDuration);

        if (enemy != null)
        {
            enemy.PerformFinisher();
        }

        playerMovement.isFinishing = false;
        isFinishing = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, finisherRange);
        Gizmos.DrawWireSphere(transform.position, approachDistance);
    }

    public void TriggerFinisherImpact()
    {
        if (hasTriggeredImpact)
            return;

        hasTriggeredImpact = true;

        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }

    public void KnockBack()
    {
        if (enemyTarget == null) return;

        Vector2 knockbackDirection = playerMovement.IsFacingRight ? Vector2.right : Vector2.left;

        enemyTarget.ApplyFinisherImpact(knockbackDirection);
    }
}
