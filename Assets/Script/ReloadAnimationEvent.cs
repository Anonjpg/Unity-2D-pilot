using UnityEngine;

public class ReloadAnimationEvent : MonoBehaviour
{
    public AimController aimController;

    public void OnReloadAnimationEvent()
    {
        if (aimController != null)
        {
            aimController.FinishReload();
        }
    }
}
