using UnityEngine;
using UnityEngine.InputSystem;

public class HistoricalFragment : MonoBehaviour
{
    [Header("Historical Record")]
    public string historicalDate;

    [TextArea(5, 12)]
    public string content;

    [Header("UI")]
    [SerializeField] private HistoryUI historyUI;
    [SerializeField] private GameObject Interaksi;
    [SerializeField] private PromptFade promptFade;

    private bool playerInRange;

    private void Update()
    {
        bool canShowPrompt =
            playerInRange &&
            historyUI != null &&
            !historyUI.IsOpen;

        if (promptFade != null)
        {
            if (canShowPrompt)
                promptFade.Show();
            else
                promptFade.Hide();
        }

        if (!playerInRange || historyUI == null)
            return;

        if (historyUI.IsOpen)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            historyUI.ShowHistory(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
