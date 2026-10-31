using UnityEngine;
using UnityEngine.InputSystem;

public class AimMouseRight : MonoBehaviour
{
    [Header("Settings")]
    [Range(0f, 89f)]
    public float maxAimAngle = 65f;

    void Update()
    {
        if (Camera.main == null || Mouse.current == null)
            return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        float depth = -Camera.main.transform.position.z;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreen.x, mouseScreen.y, depth)
        );

        Vector2 direction =
            (Vector2)mouseWorld - (Vector2)transform.position;

        float mouseAngle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float relativeAngle =
            Mathf.DeltaAngle(0f, mouseAngle);

        relativeAngle = Mathf.Clamp(
            relativeAngle,
            -maxAimAngle,
            maxAimAngle
        );

        transform.rotation = Quaternion.Euler(
            0f, 0f, relativeAngle
        );
    }
}
