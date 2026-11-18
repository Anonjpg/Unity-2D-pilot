using UnityEngine;
using System.Collections;
using UnityEditor.U2D.Sprites;

public class EnemyFlash : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float flashduration = 0.1f;

    private Color originalColor = Color.white;
    private Coroutine flashCoroutine;

    void Start()
    {
        originalColor = spriteRenderer.color;
    }

    public void Flash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashduration);

        spriteRenderer.color = originalColor;

        flashCoroutine = null;
    }
}
