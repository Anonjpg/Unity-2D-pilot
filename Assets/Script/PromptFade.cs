
using UnityEngine;

public class PromptFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 5f;

    private bool shouldShow;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show()
    {
        shouldShow = true;
    }

    public void Hide()
    {
        shouldShow = false;
    }

    private void Update()
    {
        float targetAlpha = shouldShow ? 1f : 0f;

        canvasGroup.alpha = Mathf.MoveTowards(
            canvasGroup.alpha,
            targetAlpha,
            fadeSpeed * Time.deltaTime
        );
    }
}
