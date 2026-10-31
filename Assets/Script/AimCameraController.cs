
using UnityEngine;
using Unity.Cinemachine;

public class AimCameraController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform cameraTarget;
    public AimController aimController;
    public CinemachineCamera cinemachineCamera;
    public CinemachinePositionComposer positionComposer;

    [Header("Zoom")]
    public float normalSize = 13.69f;
    public float aimSize = 17f;
    public float smoothSpeed = 5f;

    [Header("Screen Position")]
    public Vector2 normalScreenPosition = new Vector2(0.5f, 0.5f);
    public Vector2 aimRightScreenPosition = new Vector2(0.25f, 0.5f);
    public Vector2 aimLeftScreenPosition = new Vector2(0.75f, 0.5f);

    void LateUpdate()
    {
        if (player == null || cameraTarget == null ||
            aimController == null || cinemachineCamera == null ||
            positionComposer == null)
            return;

        bool aiming = aimController.IsAiming;

        float targetSize = aiming ? aimSize : normalSize;

        var lens = cinemachineCamera.Lens;
        lens.OrthographicSize = Mathf.Lerp(
            lens.OrthographicSize,
            targetSize,
            Time.deltaTime * smoothSpeed
        );
        cinemachineCamera.Lens = lens;

        Vector2 targetScreenPosition = normalScreenPosition;

        if (aiming)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    Input.mousePosition.x,
                    Input.mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );

            bool aimingRight = mouseWorld.x >= player.position.x;

            targetScreenPosition = aimingRight
                ? aimRightScreenPosition
                : aimLeftScreenPosition;
        }

        positionComposer.Composition.ScreenPosition = Vector2.Lerp(
            positionComposer.Composition.ScreenPosition,
            targetScreenPosition,
            Time.deltaTime * smoothSpeed
        );
    }
}
