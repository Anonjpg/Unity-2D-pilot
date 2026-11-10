using System.Threading.Tasks;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    async Task Fade(float targetTransparency)
    {
        float start = canvasGroup.alpha;
        float t = 0;
        while(t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, targetTransparency, t / fadeDuration);
            await Task.Yield();
        }
        canvasGroup.alpha = targetTransparency;
    }

    public async Task FadeOut()
    {
        await Fade(1);
    }

    public async Task FadeIn()
    {
        await Fade(0);
    }
}
