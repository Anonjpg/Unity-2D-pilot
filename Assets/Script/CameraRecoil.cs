using Unity.Cinemachine;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public CinemachineImpulseSource impulseSource;

    public void ShakeCamera()
    {
        impulseSource.GenerateImpulseWithForce(0.7f);
    }
}
