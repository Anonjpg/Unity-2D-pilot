
using UnityEngine;
using Unity.Cinemachine;

public class CameraAreaSwitcher : MonoBehaviour
{
    public CinemachineConfiner2D confiner;
    public CinemachinePositionComposer positionComposer;
    private Vector3 originalDamping;

    public Collider2D exteriorBounds;
    public Collider2D interiorBounds;

    public BackgroundController[] backgroundControllers;

    void Start()
    {
        originalDamping = positionComposer.Damping;
    }
    public void SwitchToInterior()
    {
        // Simpan posisi parallax sebelum masuk rumah
        foreach (BackgroundController background in backgroundControllers)
        {
            if (background != null)
            {
                background.parallaxActive = false;
            }
        }

        confiner.BoundingShape2D = interiorBounds;
        confiner.InvalidateBoundingShapeCache();

        positionComposer.Damping = Vector3.zero;
    }

    public void SwitchToExterior()
    {
        confiner.BoundingShape2D = exteriorBounds;
        confiner.InvalidateBoundingShapeCache();

        positionComposer.Damping = originalDamping;
    }
}
