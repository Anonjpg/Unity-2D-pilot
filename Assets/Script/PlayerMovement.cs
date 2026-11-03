using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    float horizontalInput;
    public float HorizontalInput => horizontalInput;
    public float moveSpeed = 25f;
    Rigidbody2D rb;
    Animator animator;
    bool isFacingRight = true;
    public bool IsFacingRight => isFacingRight;
    float jumpPower = 20f;
    bool isGrounded = false;
    public bool isAiming = false;
    public SpriteRenderer normalBodyRenderer;
    public AimController aimCont;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAiming)
        {
            horizontalInput = 0f;

            if (Camera.main != null &&
                UnityEngine.InputSystem.Mouse.current != null)
            {
                Vector2 mouseScreen =
                    UnityEngine.InputSystem.Mouse.current.position.ReadValue();

                float depth = -Camera.main.transform.position.z;

                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                    new Vector3(mouseScreen.x, mouseScreen.y, depth)
                );

                if (mouseWorld.x < transform.position.x && isFacingRight)
                {
                    isFacingRight = false;
                    normalBodyRenderer.flipX = true;
                }
                else if (mouseWorld.x > transform.position.x && !isFacingRight)
                {
                    isFacingRight = true;
                    normalBodyRenderer.flipX = false;
                }
            }

            return;
        }

        if (aimCont.isReloading)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        FlipSprite();

        if (normalBodyRenderer != null)
        {
            normalBodyRenderer.flipX = !isFacingRight;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void LateUpdate()
    {
        isRunning();
    }

    void isRunning()
    {
        if (isAiming)
        {
            horizontalInput = 0f;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            return;
        }
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Math.Abs(rb.linearVelocity.x));
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    void FlipSprite()
    {
        if (isFacingRight && horizontalInput < 0f ||
            !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isGrounded = true;
        animator.SetBool("isJumping", !isGrounded);
    }

    public void FaceDirection(bool faceRight)
    {
        if (isFacingRight == faceRight)
            return;

        isFacingRight = faceRight;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (faceRight ? 1f : -1f);
        transform.localScale = scale;
    }
}
