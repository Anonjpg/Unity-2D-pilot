using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    public Transform exteriorSpawnPoint;
    public CameraAreaSwitcher cameraAreaSwitcher;

    private bool playerNearby;
    private GameObject player;
    private Rigidbody2D playerRb;

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            ExitHouse();
        }
    }

    async void ExitHouse()
    {
        if (player == null || exteriorSpawnPoint == null)
            return;
        await ScreenFader.Instance.FadeOut();

        playerRb.position = exteriorSpawnPoint.position;
        playerRb.linearVelocity = Vector2.zero;

        cameraAreaSwitcher.SwitchToExterior();

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
