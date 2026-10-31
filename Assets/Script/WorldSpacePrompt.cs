
using UnityEngine;

public class WorldSpacePrompt : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private RectTransform promptUI;

    [Header("Position")]
    [SerializeField] private Vector3 worldOffset = new Vector3(0f, 1.2f, 0f);

    private void LateUpdate()
    {
        if (target == null || mainCamera == null || promptUI == null)
            return;

        Vector3 screenPosition =
            mainCamera.WorldToScreenPoint(
                target.position + worldOffset
            );

        if (screenPosition.z <= 0f)
        {
            promptUI.gameObject.SetActive(false);
            return;
        }

        promptUI.gameObject.SetActive(true);
        promptUI.position = screenPosition;
    }
}
