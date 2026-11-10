using System;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyChase : MonoBehaviour
{
    [Header("player detection")]
    public Transform player;
    public float detectionRange = 5f;

    [Header("enemy movement")]
    public float moveSpeed = 3f;

    [Header("Component References")]
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;

    public bool isChasing;

    void Update()
    {
        if (player == null)
        {
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        isChasing = distance <= detectionRange;

        if (isChasing)
        {
            if (player.position.x < transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null)
        {
            return;
        }

        if (isChasing)
        {
            float direction = Math.Sign(player.position.x - transform.position.x);

            rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}
