using UnityEngine;

public class DebugMenuController : MonoBehaviour
{
    public GameObject debugMenu;

    private bool isOpen = false;

    void Start()
    {
        debugMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleDebugMenu();
        }
    }

    public void ToggleDebugMenu()
    {
        isOpen = !isOpen;

        debugMenu.SetActive(isOpen);

        if (isOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = true;

        }
    }
}
