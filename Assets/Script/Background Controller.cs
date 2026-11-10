using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPos;

    public GameObject cam;
    public float parallaxEffect;

    public bool parallaxActive = true;

    void Start()
    {
        startPos = transform.position.x;
    }

    void LateUpdate()
    {
        if (!parallaxActive)
            return;

        float distance = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
    }
}
