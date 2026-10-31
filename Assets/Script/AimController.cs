using UnityEngine;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [Header("References")]
    public PlayerEquipment playerEquipment;
    public PlayerMovement playerMovement;
    public Animator playerAnimator;

    [Header("Aim Pivots")]
    public GameObject aimPivotRight;
    public GameObject aimPivotLeft;

    [Header("Aim Mouse Controllers")]
    public AimMouseRight aimMouseRight;
    public AimMouseLeft aimMouseLeft;

    [Header("Visuals")]
    public SpriteRenderer normalBodyRenderer;
    public GameObject aimBody;
    public GameObject aimRightVisuals;
    public GameObject aimLeftVisuals;

    [Header("Input")]
    public Key aimKey = Key.Q;

    [Header("Player Body Position")]
    public SpriteRenderer playerBodyRenderer;

    public Vector3 bodyRightPosition;
    public Vector3 bodyLeftPosition;

    public bool IsAiming { get; private set; }

    void Start()
    {
        SetAim(false);
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[aimKey].wasPressedThisFrame)
        {
            if (!IsAiming)
            {
                if (playerEquipment == null ||
                    !playerEquipment.HasPistol)
                {
                    Debug.LogWarning("Equip pistol terlebih dahulu!");
                    return;
                }

                SetAim(true);
            }
            else
            {
                SetAim(false);
            }
        }

        if (IsAiming)
            UpdateAimDirection();
    }

    void SetAim(bool aiming)
    {
        IsAiming = aiming;

        if (normalBodyRenderer != null)
            normalBodyRenderer.enabled = !aiming;

        if (aimBody != null)
            aimBody.SetActive(aiming);

        if (playerMovement != null)
            playerMovement.isAiming = aiming;

        if (playerAnimator != null)
            playerAnimator.SetBool("isAiming", aiming);

        if (aimMouseRight != null)
            aimMouseRight.enabled = false;

        if (aimMouseLeft != null)
            aimMouseLeft.enabled = false;

        if (!aiming)
        {
            if (aimPivotRight != null)
                aimPivotRight.SetActive(false);

            if (aimPivotLeft != null)
                aimPivotLeft.SetActive(false);

            if (aimRightVisuals != null)
                aimRightVisuals.SetActive(false);

            if (aimLeftVisuals != null)
                aimLeftVisuals.SetActive(false);
        }
        else
        {
            UpdateAimDirection();
        }
    }

    void UpdateAimDirection()
    {
        if (Camera.main == null || Mouse.current == null)
            return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        float depth = -Camera.main.transform.position.z;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreen.x, mouseScreen.y, depth)
        );

        bool aimRight = mouseWorld.x >= transform.position.x;
        FlipPlayerBody(aimRight);

        if (aimPivotRight != null)
            aimPivotRight.SetActive(aimRight);

        if (aimPivotLeft != null)
            aimPivotLeft.SetActive(!aimRight);

        if (aimRightVisuals != null)
            aimRightVisuals.SetActive(aimRight);

        if (aimLeftVisuals != null)
            aimLeftVisuals.SetActive(!aimRight);

        if (aimMouseRight != null)
            aimMouseRight.enabled = aimRight;

        if (aimMouseLeft != null)
            aimMouseLeft.enabled = !aimRight;
    }

    public void ExitAim()
    {
        if (IsAiming)
            SetAim(false);
    }

    void FlipPlayerBody(bool faceRight)
    {
        if (playerBodyRenderer == null)
            return;

        playerBodyRenderer.flipX = !faceRight;

        playerBodyRenderer.transform.localPosition =
            faceRight ? bodyRightPosition : bodyLeftPosition;
    }
}
