using UnityEngine;
using TMPro;

public class SafeUIController : MonoBehaviour
{
   public GameObject safeUI;
   public TMP_Text codeDisplay;
   public TMP_Text statusText;

   private string enteredCode = "";
   private string correctCode = "222";

    public bool isOpen = false;
   public bool isUnlocked = false;

   void Start()
    {
        safeUI.SetActive(false);
    }

    public void OpenSafeUI()
    {
        if (isUnlocked)
        {
            return;
        }

        safeUI.SetActive(true);
        enteredCode = "";
        codeDisplay.text = "___";
        statusText.text = "";

        isOpen = true;
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseSafeUI()
    {
        safeUI.SetActive(false);
        isOpen = false;

        Cursor.visible = true;
    }

    public void AddDigit(int digit)
    {
        if (enteredCode.Length >= 3)
        {
            return;
        }

        enteredCode += digit.ToString();
        codeDisplay.text = enteredCode;
    }
    public void ClearCode()
    {
        enteredCode = "";
        codeDisplay.text = "___";
        statusText.text = "";
    }

    public void CheckCode()
    {
        if (enteredCode == correctCode)
        {
            statusText.text = "Unlocked!";
            isUnlocked = true;
            codeDisplay.text = "222";
        }   
        else
        {
            statusText.text = "Incorrect Code!";
            enteredCode = "";
            codeDisplay.text = "___";
        }
    }

    void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseSafeUI();
        }
    }
}
