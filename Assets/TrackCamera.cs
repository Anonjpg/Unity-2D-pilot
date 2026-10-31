using UnityEngine;

public class TrackCamera : MonoBehaviour
{
    float horizontalInput;
    float moveSpeed = 25f;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }
}
