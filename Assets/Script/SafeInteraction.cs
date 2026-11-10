using UnityEngine;

public class SafeInteraction : MonoBehaviour
{
    public SafeUIController safeUIController;

    private bool playerInRange = false;


    void Update()
    {
        if (playerInRange &&
            !safeUIController.isOpen &&
            !safeUIController.isUnlocked &&
            Input.GetKeyDown(KeyCode.E))
        {
            safeUIController.OpenSafeUI();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
