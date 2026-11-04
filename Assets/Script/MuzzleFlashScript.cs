using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class MuzzleFlashScript : MonoBehaviour
{
    public Light2D muzzleFlashLight;
    public float FlashDurration = 0.08f;

    Coroutine flashCoroutine;

    void Start()
    {
        muzzleFlashLight.enabled = false;
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
        muzzleFlashLight.enabled = true;
        yield return new WaitForSeconds(FlashDurration);
        muzzleFlashLight.enabled = false;
        flashCoroutine = null;
    }
}
