using UnityEngine;

public class EnteranceTrigger : MonoBehaviour
{
    public Transform interiorSpawnPoint;
    public CameraAreaSwitcher cameraAreaSwitcher;

    private bool playerNearby;
    private GameObject player;
    private Rigidbody2D playerRb;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            EnterHouse();
        }
    }

    async void EnterHouse()
    {
        if (player == null || interiorSpawnPoint == null)
            return;
        await ScreenFader.Instance.FadeOut();

        playerRb.position = interiorSpawnPoint.position;
        playerRb.linearVelocity = Vector2.zero;

        cameraAreaSwitcher.SwitchToInterior();

        await ScreenFader.Instance.FadeIn();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerRb = player.GetComponent<Rigidbody2D>();

            playerNearby = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }
}
