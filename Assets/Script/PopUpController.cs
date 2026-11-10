using UnityEngine;

public class PopUpController : MonoBehaviour
{
   [Header("Pop-Up Settings")]
   [SerializeField] private PromptFade promptFade;
   [SerializeField] private GameObject popUpPanel;
   
   private bool playerinRange;

   void Update()
    {
          bool canShowPrompt =
            playerinRange &&
            promptFade != null;

        if (promptFade != null)
        {
            if (canShowPrompt)
                promptFade.Show();
            else
                promptFade.Hide();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinRange = false;

        }
    }
}
